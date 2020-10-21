using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Quartz.Impl.AdoJobStore;

namespace TreeOfAKind.API.SeedWork
{
    public static class Helpers
    {
        public static string GetFirebaseUserAuthId(this HttpContext httpContext)
        {
            if (httpContext.User.Identity is ClaimsIdentity identity)
            {
                return identity.FindFirst("user_id").Value;
            }

            return null;
        }
    }
}