using System.Linq;
using System.Net;
using Microservices.Core;
using Microsoft.AspNetCore.Identity;

namespace Identity.Api.Core
{
    public static class ApiExceptionExtension
    {
        public static ApiException ParseIdentityError(this ApiException ex, IdentityResult result)
        {
            var firstError = result.Errors.FirstOrDefault();
            return firstError == null
                ? new ApiException(HttpStatusCode.BadRequest, "Error Occured")
                : new ApiException(firstError.Code, firstError.Description);
        }
    }
}