using System;
using System.Data;
using System.Net.Mail;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TreeOfAKind.API.SeedWork;
using TreeOfAKind.Application.Configuration.Data;
using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Domain.UserProfiles.Rules;
using TreeOfAKind.Infrastructure.Database;

namespace TreeOfAKind.API.Configuration
{
    public class UserProfileMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserAuthIdUniquenessChecker _userAuthIdUniquenessChecker;

        public UserProfileMiddleware(
            RequestDelegate next, IUserAuthIdUniquenessChecker userAuthIdUniquenessChecker)
        {
            this._next = next;
            _userAuthIdUniquenessChecker = userAuthIdUniquenessChecker;
        }

        public async Task Invoke(HttpContext context, TreesContext treesContext)
        {
            await GenerateOrUpdateDomainUserProfile(context, treesContext);

            await _next.Invoke(context);
        }

        private async Task GenerateOrUpdateDomainUserProfile(HttpContext context, TreesContext treesContext)
        {
            var mail = context.GetUserEmail();
            var userAuthId = context.GetFirebaseUserAuthId();

            if (!string.IsNullOrEmpty(userAuthId))
            {

                var user =
                    await treesContext.Users.FirstOrDefaultAsync(u => u.UserAuthId == userAuthId);

                if (user is null)
                {
                    user = UserProfile.CreateUserProfile(userAuthId, new MailAddress(mail), null, null, null,
                        _userAuthIdUniquenessChecker);

                    await treesContext.Users.AddAsync(user);
                }
                else if (!Equals(user.ContactEmailAddress?.Address, mail))
                {
                    user.UpdateContactEmailAddress(new MailAddress(mail));
                }

                await treesContext.SaveChangesAsync();
            }
        }
    }
}
