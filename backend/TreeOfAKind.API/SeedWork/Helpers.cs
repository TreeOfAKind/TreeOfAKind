using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TreeOfAKind.API.SeedWork
{
    public static class Helpers
    {
        public static string GetFirebaseUserAuthId(this HttpContext httpContext)
        {
            if (httpContext.User.Identity is ClaimsIdentity identity)
            {
                return identity.FindFirst("user_id")?.Value;
            }

            return null;
        }

        public static string GetUserEmail(this HttpContext httpContext)
        {
            if (httpContext.User.Identity is ClaimsIdentity identity)
            {
                return identity.FindFirst(ClaimTypes.Email)?.Value;
            }

            return null;
        }
    }
}
