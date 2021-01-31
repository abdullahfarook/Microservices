using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using EventBus.Common.Abstractions;
using Identity.Api.Core;
using Identity.Api.DataAccess;
using Identity.Api.DataAccess.Model;
using Identity.Api.Events;
using Identity.Api.Services;
using Identity.Api.ViewModel;
using Microservices.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Authorize]
    public class AuthController : BaseController
    {
        private readonly IdentityDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IAccountManager _accountManager;
        private readonly int _pageSize = 20;
        private readonly IEventBus _eventBus;

        public AuthController(
        IAccountManager accountManager,
        IEmailSender emailSender,
        IdentityDbContext context,
        IEventBus eventBus,
        IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _emailSender = emailSender;
            _accountManager = accountManager;
            _eventBus = eventBus;
        }
        [HttpGet("permissions")]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<List<PermissionGroup>> GetPermissions()
        {
            return Ok(ApplicationPermissions.GetAllPermissions());
        }

        [HttpGet("email/confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailAddress(string token, string email)
        {
            var user = await _accountManager.GetUserByEmailAsync(email);
            if (user == null) throw new ApiException(HttpStatusCode.NotFound, "Email is incorrect");

            var reset = await _accountManager.ResetAccessFailedCountAsync(user);
            if (!reset.Succeeded) throw new ApiException().ParseIdentityError(reset);

            if (await _accountManager.IsEmailConfirmedAsync(user))
                return Redirect($"{Extensions.AdminUrl}/login");

            var result = await _accountManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) throw new ApiException().ParseIdentityError(result);

            return Redirect($"{Extensions.AdminUrl}/login?EmailConfirmed=true");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login(LoginViewModel model)
        {
            var user = await _accountManager.GetUserByUserNameAsync(model.UserName);
            var token = await ValidateUserAndGenerateToken(user, false, model.Password);
            return Ok(token);

        }

        private async Task<string> ValidateUserAndGenerateToken(User user, bool isExternal = true, string password = null)
        {
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "Username or Password is incorrect");

            if (user.IsBlocked)
                throw new ApiException(HttpStatusCode.Forbidden, "You are blocked by our system! Contact Us for more information");

            if (await _accountManager.IsUserLockedAsync(user))
                throw new ApiException(HttpStatusCode.Forbidden, "Too many failed attempts! Try after sometime.");

            if (!isExternal)
            {
                if (!await _accountManager.IsPasswordValidAsync(user, password))
                    throw new ApiException(HttpStatusCode.BadRequest, "Username or Password is incorrect");
            }
            if (!await _accountManager.IsEmailConfirmedAsync(user))
                throw new ApiException(HttpStatusCode.Unauthorized, "Go To your email and click on the Email confirmation link");

            return await _accountManager.GenerateUserToken(user);


        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserViewModel>> Register(RegisterViewModel user)
        {
            var appUser = Mapper.Map<User>(user);
            appUser.FullName = user.FirstName + " " + appUser.LastName;
            var vm = await CreateNewUser(appUser, new List<string> { Roles.Contributor }, user.Password);

            return Ok(_mapper.Map<UserViewModel>(vm));
        }

        [HttpPost("password/forget")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var user = await _accountManager.GetUserByEmailAsync(model.Email);
            if (user == null) throw new ApiException(HttpStatusCode.NotFound, "Incorrect Email");

            var token = await _accountManager.GeneratePasswordResetTokenAsync(user);
            var resetUrl = $"{Extensions.AdminUrl}/password/reset?token={HttpUtility.UrlEncode(token)}&email={user.Email}";

            _emailSender.SendEmail(user.FullName, user.Email,
                "Reset Password", new ForgetPassword(user.FullName, resetUrl));

            return Ok();
        }

        [HttpPut("password")]
        public async Task<ActionResult<UserViewModel>> UpdatePassword(UpdatePasswordViewModel vm)
        {
            var userId = GetSession().Id;

            var model = await _accountManager.GetUserByIdAsync(userId);

            if (model == null)
                throw new ApiException(HttpStatusCode.NotFound, "Incorrect email");

            var result = await _accountManager.UpdatePasswordAsync(model, vm.Password, vm.NewPassword);
            if (!result.Succeeded) throw new ApiException().ParseIdentityError(result);

            return Ok(_mapper.Map<UserViewModel>(model));
        }

        [HttpDelete("users/{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<UserViewModel>> DeleteUser(long id)
        {

            var user = await _accountManager.GetUserByIdAsync(id);
            if (user == null) throw new ApiException(HttpStatusCode.NotFound, "Incorrect EventId");

            var result = await _accountManager.DeleteUserAsync(user);
            if (!result.Succeeded) throw new ApiException().ParseIdentityError(result);

            return Ok(_mapper.Map<UserViewModel>(user));
        }

        [HttpPost("password/reset")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _accountManager.GetUserByEmailAsync(model.Email);
            if (user == null) throw new ApiException(HttpStatusCode.NotFound, "Incorrect Email!");

            var token = HttpUtility.UrlDecode(model.Token)?.Replace(" ", "+");
            var result = await _accountManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (!result.Succeeded) throw new ApiException().ParseIdentityError(result);

            return Ok();
        }

        [HttpGet("external")]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider)
        {
            var authenticationProperties = _accountManager.GenerateExternalLink(provider, Url.Action(nameof(ExternalLoginHandler)));
            return Challenge(authenticationProperties, provider);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginHandler()
        {
            var user = await _accountManager.GetExternalUser();
            var token = await ValidateUserAndGenerateToken(user);
            return new ContentResult
            {
                Content = Templates.Authentication.RedirectToken(token),
                ContentType = "text/html",
            };
        }

        [HttpGet("roles/{pageNumber:int}/{pageSize:int}")]
        [Authorize(Permissions.Roles.View)]
        public async Task<ActionResult<List<RoleViewModel>>> GetRoles(int pageNumber, int pageSize, string searchTerm, string order, string field)
        {
            return Ok(_mapper.Map<List<RoleViewModel>>(await _accountManager.GetRoles(pageNumber, pageSize, searchTerm, order, field)));
        }

        [HttpGet("roles/count")]
        public async Task<ActionResult<int>> GetRolesCount()
        {
            return Ok(await _accountManager.GetRolesCount());
        }

        [HttpGet("roles/{id}")]
        [Authorize(Permissions.Roles.View)]
        public async Task<ActionResult<RoleViewModel>> GetRoleById(long id)
        {
            var model = await _accountManager.GetRoleByIdWithClaimsAsync(id);

            //if (!(await _authorizationService.AuthorizeAsync(this.User, appRole?.Name ?? "", Authorization.Policies.ViewRoleByRoleNamePolicy)).Succeeded)
            //    return new ChallengeResult();

            if (model == null)
                return NotFound(id);
            var vm = _mapper.Map<RoleViewModel>(model);
            var claims = ApplicationPermissions.AllPermissions
                .Where(p => model.RoleClaims
                    .Select(rc => rc.ClaimValue)
                    .Contains(p.Value));
            vm.Claims = _mapper.Map<List<ClaimViewModel>>(claims);
            return Ok(vm);
        }


        //[HttpGet("roles/name/{name}")]
        //public async Task<ActionResult<RoleViewModel>> GetRoleByName(string name)
        //{
        //    //if (!(await _authorizationService.AuthorizeAsync(this.User, name, Authorization.Policies.ViewRoleByRoleNamePolicy)).Succeeded)
        //    //    return new ChallengeResult();


        //    //RoleViewModel roleVM = await GetRoleViewModelHelper(name);

        //    //if (roleVM == null)
        //    //    return NotFound(name);

        //    var roleVM = await _accountManager.GetRoleByNameAsync(name);
        //    return Ok(roleVM);
        //}


        //[HttpGet("roles")]
        //[Authorize(Authorization.Policies.ViewAllRolesPolicy)]
        //[ProducesResponseType(200, Type = typeof(List<RoleViewModel>))]
        //public async Task<IActionResult> GetRoles()
        //{
        //    return await GetRoles(-1, -1);
        //}


        //[HttpGet("roles/{pageNumber:int}/{pageSize:int}")]
        //[Authorize(Authorization.Policies.ViewAllRolesPolicy)]
        //[ProducesResponseType(200, Type = typeof(List<RoleViewModel>))]
        //public async Task<IActionResult> GetRoles(int pageNumber, int pageSize)
        //{
        //    var roles = await _accountManager.GetRolesLoadRelatedAsync(pageNumber, pageSize);
        //    return Ok(Mapper.Map<List<RoleViewModel>>(roles));
        //}


        [HttpPut("roles/{id}")]
        [Authorize(Permissions.Roles.Manage)]
        //[Authorize(Authorization.Policies.ManageAllRolesPolicy)]
        public async Task<ActionResult<RoleViewModel>> UpdateRole(long id, [FromBody] RoleViewModel role)
        {
            var appRole = await _accountManager.GetRoleByIdAsync(id);

            if (appRole == null)
                return NotFound(id);

            if (string.IsNullOrWhiteSpace(role.Name))
                role.Name = appRole.Name;

            var currentRole = Mapper.Map(role, appRole);

            //var result = await _accountManager.UpdateRoleAsync(appRole, role.Permissions?.Select(p => p.Value).ToArray());
            var result = await _accountManager.UpdateRoleAsync(currentRole, role.Claims.Select(x => x.Value));
            if (!result.Succeeded)
                return NoContent();

            return Ok(currentRole);
        }


        [HttpPost("roles")]
        [Authorize(Permissions.Roles.Manage)]
        //[Authorize(Authorization.Policies.ManageAllRolesPolicy)]
        public async Task<ActionResult<RoleViewModel>> CreateRole(RoleViewModel role)
        {
            if (role == null)
                return BadRequest($"{nameof(role)} cannot be null");


            Role appRole = Mapper.Map<Role>(role);

            //var result = await _accountManager.CreateRoleAsync(appRole, role.Permissions?.Select(p => p.Value).ToArray());
            //if (result.Item1)
            //{
            //    RoleViewModel roleVM = await GetRoleViewModelHelper(appRole.Name);
            //    return CreatedAtAction(GetRoleByIdActionName, new { id = roleVM.EventId }, roleVM);
            //}

            //AddErrors(result.Item2);
            var result = await _accountManager.CreateRoleAsync(appRole);
            if (!result.Succeeded)
                throw new Exception("The following errors occurred whilst creating role: " + string.Join(", ", result.Errors));

            return Ok(role);
        }


        [HttpDelete("roles/{id}")]
        [Authorize(Permissions.Roles.Manage)]
        //[Authorize(Authorization.Policies.ManageAllRolesPolicy)]]
        public async Task<ActionResult<RoleViewModel>> DeleteRole(long id)
        {
            //if (!await _accountManager.TestCanDeleteRoleAsync(id))
            //    return BadRequest("Role cannot be deleted. Remove all users from this role and try again");


            //RoleViewModel roleVM = null;

            Role role = await _accountManager.GetRoleByIdAsync(id);

            //if (appRole != null)
            //    roleVM = await GetRoleViewModelHelper(appRole.Name);


            //if (roleVM == null)
            //    return NotFound(id);

            var result = await _accountManager.DeleteRoleAsync(role);
            if (!result.Succeeded)
                throw new Exception("The following errors occurred whilst deleting role: " + string.Join(", ", result.Errors));


            return Ok(_mapper.Map<RoleViewModel>(role));
        }


        [HttpGet("users/{pageNumber:int}/{pageSize:int}")]
        [Authorize(Permissions.Users.View)]
        //[Authorize(Authorization.Policies.ViewAllUsersPolicy)]
        public async Task<ActionResult<List<UserViewModel>>> GetUsers(int pageNumber, int pageSize, string name, string order, string field)
        {
            if (pageSize > _pageSize) throw new ApiException(HttpStatusCode.BadRequest, "Too large page size");
            var currentAdminId = GetSession().Id;
            var isAdmin = GetSession().Roles.Contains(Roles.Admin);
            var vm = _mapper.Map<List<UserViewModel>>(await _accountManager.GetUsers(pageNumber, pageSize, currentAdminId, isAdmin, name, order, field));
            return Ok(vm);
        }

        [HttpGet("users/me")]
        public async Task<ActionResult<UserViewModel>> GetCurrentUserDetail()
        {
            var model = await _context.Users.FirstOrDefaultAsync(x => x.Id == GetSession().Id);
            var roles = await _accountManager.GetUserRolesAsync(model);
            var userVm = _mapper.Map<UserViewModel>(model);
            userVm.Roles = roles.ToList();
            return Ok(userVm);
        }
        [HttpGet("users/count")]
        public async Task<ActionResult<int>> GetUsersCount()
        {
            var isAdmin = GetSession().Roles.Contains("Admin");
            return Ok(await _accountManager.GetUsersCount(GetSession().Id, isAdmin));
        }
        [HttpPut("users/me")]
        public async Task<ActionResult<UserViewModel>> UpdateCurrentUser([FromBody] UserViewModel vm)
        {
            var model = await _accountManager.GetUserByIdAsync(GetSession().Id);

            if (model == null)
                throw new ApiException(HttpStatusCode.NotFound, "Incorrect EventId");
            model = _mapper.Map<User>(vm);
            model.FullName = vm.FirstName + " " + vm.LastName;
            model.LatestUpdatedOn = DateTime.UtcNow;

            var result = await _accountManager.UpdateUserAsync(model);
            if (!result.Succeeded) throw new ApiException().ParseIdentityError(result);

            return Ok(_mapper.Map<UserViewModel>(model));
        }

        private async Task<UserViewModel> CreateNewUser(User user, List<string> roles, string password)
        {
            var result = await _accountManager.CreateUserAsync(user, roles, password);

            if (!result.Succeeded) throw new ApiException().ParseIdentityError(result);

            var emailToken = await _accountManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmUrl = Url.Action("ConfirmEmailAddress", "Auth", new
            {
                Token = emailToken,
                user.Email
            }, Request.Scheme);

            _emailSender.SendEmail(user.FullName, user.Email,
            "Confirm Your Email", new SignUpVerification(user.FullName, confirmUrl));

            var userVm = Mapper.Map<UserViewModel>(user);
            userVm.Roles = roles;
            return userVm;
        }

        [HttpPost("users")]
        [Authorize(Permissions.Users.Manage)]
        public async Task<ActionResult<UserViewModel>> CreateUser(UserViewModel user)
        {
            User appUser = Mapper.Map<User>(user);
            appUser.FullName = user.FirstName + " " + user.LastName;
            appUser.CreatedOn = DateTime.UtcNow;
            appUser.LatestUpdatedOn = DateTime.UtcNow;
            appUser.IsBlocked = false;
            return Ok(await CreateNewUser(appUser, user.Roles, user.Password));
        }

        [HttpGet("users/{id}")]
        [Authorize(Permissions.Users.View)]
        public async Task<ActionResult<UserViewModel>> GetUserById(long id)
        {
            //if (!(await _authorizationService.AuthorizeAsync(this.User, id, AccountManagementOperations.Read)).Succeeded)
            //    return new ChallengeResult();

            var userAndRoles = await _accountManager.GetUserAndRolesAsync(id);

            var userAndRolesVm = Mapper.Map<UserViewModel>(userAndRoles.Item1);
            userAndRolesVm.Roles = userAndRoles.Item2.ToList();

            return Ok(userAndRolesVm);
        }

        [HttpPut("users/{id}")]
        [Authorize(Permissions.Users.Manage)]
        public async Task<ActionResult<UserViewModel>> UpdateUser([FromBody] UserViewModel vm, long id)
        {
            var model = await _accountManager.GetUserByIdAsync(id);

            if (model == null)
                throw new ApiException(HttpStatusCode.NotFound, "Incorrect EventId");

            model = _mapper.Map(vm, model);
            model.LatestUpdatedOn = DateTime.UtcNow;
            model.FullName = vm.FirstName + " " + vm.LastName;

            var result = await _accountManager.UpdateUserAsync(model);
            if (!result.Succeeded) throw new ApiException().ParseIdentityError(result);

            result = await _accountManager.UpdateUserRolesAsync(model, vm.Roles);
            if (!result.Succeeded)
                throw new ApiException().ParseIdentityError(result);
            var userVm = _mapper.Map<UserViewModel>(model);
            userVm.Roles = vm.Roles;

            var @event = _mapper.Map<UpdateUserIntegrationEvent>(model);
            _eventBus.Publish(@event);

            return Ok(userVm);
        }

        [HttpPut("users/unblock/{id}")]
        [Authorize(Permissions.Users.Manage)]
        //[Authorize(Authorization.Policies.ManageAllUsersPolicy)]
        public async Task<ActionResult<UserViewModel>> UnblockUser(long id)
        {
            User appUser = await _accountManager.GetUserByIdAsync(id);

            if (appUser == null)
                return NotFound(id);

            appUser.IsBlocked = false;
            appUser.LockoutEnd = null;
            var result = await _accountManager.UpdateUserAsync(appUser);
            if (!result.Succeeded)
                throw new Exception("The following errors occurred whilst unblocking user: " + string.Join(", ", result.Errors));

            var userVm = _mapper.Map<UserViewModel>(appUser);
            return Ok(userVm);
        }

        [HttpPut("users/block/{id}")]
        [Authorize(Permissions.Users.Manage)]
        //[Authorize(Authorization.Policies.ManageAllUsersPolicy)]
        public async Task<ActionResult<UserViewModel>> BlockUser(long id)
        {
            User appUser = await _accountManager.GetUserByIdAsync(id);

            if (appUser == null)
                return NotFound(id);

            appUser.IsBlocked = true;

            var result = await _accountManager.UpdateUserAsync(appUser);
            if (!result.Succeeded)
                throw new Exception("The following errors occurred whilst unblocking user: " + string.Join(", ", result.Errors));
            var userVm = _mapper.Map<UserViewModel>(appUser);
            return Ok(userVm);
        }
    }
}