using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Api.Core;
using Identity.Api.DataAccess;
using Identity.Api.DataAccess.Model;
using Microservices.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Services
{
    public interface IAccountManager
    {
        Task<bool> IsPasswordValidAsync(User user, string password);
        Task<IdentityResult> CreateRoleAsync(Role role);
        Task<IdentityResult> DeleteRoleAsync(Role role);
        Task<IdentityResult> DeleteRoleAsync(string roleName);
        Task<IdentityResult> DeleteUserAsync(User user);
        Task<IdentityResult> DeleteUserAsync(long userId);
        Task<Role> GetRoleByIdAsync(long roleId);
        Task<Role> GetRoleByIdWithClaimsAsync(long roleId);
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<List<Tuple<User, string[]>>> GetUsersAndRolesAsync();
        Task<Tuple<User, string[]>> GetUserAndRolesAsync(long userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(long userId);
        Task<User> GetUserByUserNameAsync(string userName);
        Task<IList<string>> GetUserRolesAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string newPassword);
        Task<IdentityResult> UpdatePasswordAsync(User user, string currentPassword, string newPassword);
        Task<IdentityResult> UpdateRoleAsync(Role role, IEnumerable<string> claims);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IdentityResult> UpdateUserRolesAsync(User user, IEnumerable<string> roles);
        Task<string> GenerateUserToken(User user);
        Task<IdentityResult> CreateUserAsync(User user, IEnumerable<string> roles, string password = null);
        Task<List<User>> GetUsers(int page, int pageSize, long currentUserId, bool isAdmin, string searchTerm = null, string order = "asc", string field = "id");
        Task<List<Role>> GetRoles(int page, int pageSize, string searchTerm = null, string order = "asc", string field = "id");
        Task<int> GetUsersCount(long currentUserId, bool isAdmin);
        Task<int> GetRolesCount();
        Task<IdentityResult> ResetAccessFailedCountAsync(User user);
        Task<bool> IsEmailConfirmedAsync(User user);
        Task<bool> IsUserLockedAsync(User user);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        AuthenticationProperties GenerateExternalLink(string provider, string externalLoginHandler);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        Task<User> GetExternalUser();
    }
    public class AccountManager : IAccountManager
    {
        private readonly IdentityDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenGenerator _tokenGenerator;

        public AccountManager(IdentityDbContext context,
            ITokenGenerator tokenGenerator,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenGenerator = tokenGenerator;
            _signInManager = signInManager;
        }
        public async Task<User> GetUserByIdAsync(long userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }
        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {

            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<List<User>> GetUsers(int page, int pageSize, long currentUserId, bool isAdmin, string searchTerm = null, string order = "desc", string field = null)
        {
            IQueryable<User> usersQuery = _context.Users
                .OrderByDescending(x => x.CreatedOn);

            usersQuery = (isAdmin) ? 
                usersQuery.Where(u => u.Id != currentUserId) : 
                usersQuery.Where(u => u.Roles.All(x => x.RoleId != Extensions.AdminId) && u.Id != currentUserId);

            if (searchTerm != null)
                usersQuery = usersQuery.Where(u => u.FullName.ToLower().Contains(searchTerm.ToLower()));

            if (field != null)
            {
                usersQuery = (order == "asc") ?
                    usersQuery.OrderByProperty(field.ToTitleCase()) :
                    usersQuery.OrderByPropertyDescending(field.ToTitleCase());
            }

            if (page > -1)
                usersQuery = usersQuery.Skip(page * pageSize);

            if (pageSize > 0)
                usersQuery = usersQuery.Take(pageSize);

            return await usersQuery
                .ToListAsync();
        }
        public async Task<IdentityResult> CreateUserAsync(User user, IEnumerable<string> roles, string password = null)
        {
            var result = password == null ?
                await _userManager.CreateAsync(user) :
                await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return result;


            user = await _userManager.FindByNameAsync(user.UserName);

            try
            {
                result = await _userManager.AddToRolesAsync(user, roles.Distinct());
            }
            catch
            {
                await DeleteUserAsync(user);
                throw new Exception($"Error in inserting Role to User : {user.Email}");
            }

            if (!result.Succeeded)
            {
                await DeleteUserAsync(user);
                return result;
            }

            return IdentityResult.Success;
        }
        public async Task<List<Role>> GetRoles(int page, int pageSize, string searchTerm = null, string order = "asc", string field = null)
        {
            IQueryable<Role> rolesQuery = _context.Roles
                .OrderBy(u => u.Id);

            if (searchTerm != null)
                rolesQuery = rolesQuery.Where(u => u.Name.ToLower().Contains(searchTerm.ToLower()));

            if (field != null)
            {
                rolesQuery = (order == "asc") ?
                    rolesQuery.OrderByProperty(field.ToTitleCase()) :
                    rolesQuery.OrderByPropertyDescending(field.ToTitleCase());
            }

            if (page != -1)
                rolesQuery = rolesQuery.Skip(page * pageSize);

            if (pageSize != -1)
                rolesQuery = rolesQuery.Take(pageSize);

            var roles = rolesQuery
                .ToListAsync();
            return await roles;
        }
        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }
        public Task<List<Tuple<User, string[]>>> GetUsersAndRolesAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<Tuple<User, string[]>> GetUserAndRolesAsync(long userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            if (user == null)
                return null;

            var roles = await _context.UserRoles
                 .Where(r => r.UserId == userId)
                .Join(_context.Roles,
                uRole => uRole.RoleId,
                role => role.Id,
                (uRole, role) => role)
                .Select(r => r.Name)
                .ToArrayAsync();

            return Tuple.Create(user, roles);
        }
        public async Task<IdentityResult> UpdateRoleAsync(Role role, IEnumerable<string> claims)
        {
            claims = claims.ToArray();
            if (claims.Any())
            {

                string[] invalidClaims = claims.Where(c => ApplicationPermissions.GetPermissionByValue(c) == null).ToArray();
                if (invalidClaims.Any())
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = HttpStatusCode.NotFound.ToString(),
                        Description = "The following claim types are invalid: " + string.Join(", ", invalidClaims)
                    });
            }

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
                return result;

            var roleClaims = (await _roleManager.GetClaimsAsync(role))
                .ToArray();

            if (claims.Any())
            {
                var roleClaimValues = roleClaims.Select(c => c.Value).ToArray();


                var claimsToRemove = roleClaimValues.Except(claims).ToArray();
                var claimsToAdd = claims
                    .Except(roleClaimValues)
                    .Distinct()
                    .ToArray();


                if (claimsToRemove.Any())
                {
                    foreach (string claim in claimsToRemove)
                    {
                        result = await _roleManager.RemoveClaimAsync(role, roleClaims.FirstOrDefault(c => c.Value == claim));
                        if (!result.Succeeded)
                            return result;
                    }
                }


                if (claimsToAdd.Any())
                {
                    foreach (string claim in claimsToAdd)
                    {
                        result = await _roleManager.AddClaimAsync(role, new Claim(CustomClaimsTypes.Permission, ApplicationPermissions.GetPermissionByValue(claim)));
                        if (!result.Succeeded)
                            return result;
                    }
                }
            }
            else
            {
                // Remove all Claims
                if (roleClaims.Any())
                {
                    foreach (var claim in roleClaims)
                    {
                        result = await _roleManager.RemoveClaimAsync(role, claim);
                        if (!result.Succeeded)
                            return result;
                    }
                }
            }

            return result;
        }
        public async Task<IdentityResult> UpdateUserRolesAsync(User user, IEnumerable<string> roles)
        {
            var result = IdentityResult.Success;
            roles = roles.ToArray();
            var userRoles = await _userManager.GetRolesAsync(user);
            string[] rolesToRemove;
            string[] rolesToAdd = {};

            if (roles.Any())
            {
                rolesToRemove = userRoles.Except(roles).ToArray();
                rolesToAdd = roles.Except(userRoles).Distinct().ToArray();

            }
            else
            {
                rolesToRemove = userRoles.ToArray();
            }

            if (rolesToRemove.Any())
            {
                result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!result.Succeeded)
                    return result;
            }

            if (rolesToAdd.Any())
            {
                result = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!result.Succeeded)
                    return result;
            }

            return result;
        }
        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }
        public async Task<string> GenerateUserToken(User user)
        {
            var dbRoles = await (from role in _context.Roles
                    .Include(x => x.RoleClaims)
                                 join userRole in _context.UserRoles on role.Id equals userRole.RoleId
                                 where userRole.UserId.Equals(user.Id)
                                 select role).ToListAsync();



            var roles = new List<string>();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
            };
            foreach (var role in dbRoles)
            {
                claims.AddRange(role.RoleClaims.Select(rc => rc.ToClaim()));
                roles.Add(role.Name);
            }
            return _tokenGenerator.GenerateToken(claims, roles.ToArray(), DateTime.UtcNow.AddDays(7));
        }
        public async Task<IdentityResult> ResetPasswordAsync(User user, string newPassword)
        {
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            return await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        }
        public async Task<IdentityResult> UpdatePasswordAsync(User user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }
        public async Task<bool> IsPasswordValidAsync(User user, string password)
        {
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                await _userManager.AccessFailedAsync(user);
                return false;
            }
            await _userManager.ResetAccessFailedCountAsync(user);
            return true;
        }
        public async Task<IdentityResult> DeleteUserAsync(long userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user != null)
                return await DeleteUserAsync(user);

            return IdentityResult.Success;
        }
        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }
        public async Task<Role> GetRoleByIdAsync(long roleId)
        {
            return await _roleManager.FindByIdAsync(roleId.ToString());
        }

        public async Task<Role> GetRoleByIdWithClaimsAsync(long roleId)
        {
            return await _context.Roles.Include(x => x.RoleClaims)
                .SingleOrDefaultAsync(x => x.Id == roleId);
        }
        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }
        public async Task<IdentityResult> CreateRoleAsync(Role role)
        {
            return await _roleManager.CreateAsync(role);
        }
        public async Task<IdentityResult> UpdateRoleAsync(Role role)
        {
            return await _roleManager.UpdateAsync(role);
        }
        public async Task<IdentityResult> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role != null)
                return await DeleteRoleAsync(role);

            return IdentityResult.Success;
        }
        public async Task<IdentityResult> DeleteRoleAsync(Role role)
        {
            return await _roleManager.DeleteAsync(role);
        }
        public async Task<int> GetUsersCount(long currentUserId, bool isAdmin)
        {
            IQueryable<User> usersQuery = _context.Users;

            usersQuery = (isAdmin) ?
                usersQuery.Where(u => u.Id != currentUserId) :
                usersQuery.Where(u => u.Roles.All(x => x.RoleId != Extensions.AdminId) && u.Id != currentUserId);

            return await usersQuery.CountAsync();
        }
        public async Task<int> GetRolesCount()
        {
            return await _context.Roles.CountAsync();
        }
        public async Task<IdentityResult> ResetAccessFailedCountAsync(User user)
        {
            return await _userManager.ResetAccessFailedCountAsync(user);
        }
        public async Task<bool> IsEmailConfirmedAsync(User user)
        {
            return await _userManager.IsEmailConfirmedAsync(user);
        }
        public async Task<bool> IsUserLockedAsync(User user)
        {
            return await _userManager.IsLockedOutAsync(user);
        }
        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }
        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }
        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string NewPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, NewPassword);
        }
        public AuthenticationProperties GenerateExternalLink(string provider, string ExternalLoginHandler)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, ExternalLoginHandler);
        }
        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await _signInManager.GetExternalLoginInfoAsync();
        }
        public async Task<User> GetExternalUser()
        {
            var info = await GetExternalLoginInfoAsync();
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var fullName = info.Principal.FindFirstValue(ClaimTypes.Name);
            var user = await GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    UserName = email,
                    FullName = fullName,
                    Email = email,
                    CreatedOn = DateTime.UtcNow,
                    LatestUpdatedOn = DateTime.UtcNow,
                };
                var result = await CreateUserAsync(user, new List<string>{ Roles.Contributor});
                if (!result.Succeeded) throw new ApiException().ParseIdentityError(result);
            }
            user.EmailConfirmed = true;
            var res = await UpdateUserAsync(user);
            if (!res.Succeeded) throw new ApiException().ParseIdentityError(res);
            return user;
        }
    }
}
