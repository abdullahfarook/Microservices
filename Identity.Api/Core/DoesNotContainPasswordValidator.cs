using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Identity.Api.Core
{
    public class DoesNotContainPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser: class
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> userManager, TUser user, string password)
        {
            var userName = await userManager.GetUserNameAsync(user);
            if (userName == password)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Password cannot contain username" });
            }
            if (password.Contains("password") || password.Contains("Password"))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Password cannot contain password" });
            }
            return IdentityResult.Success;
        }
    }
}
