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
        private readonly TreesContext _treesContext;
        private readonly IUserAuthIdUniquenessChecker _userAuthIdUniquenessChecker;

        public UserProfileMiddleware(
            RequestDelegate next, TreesContext treesContext, IUserAuthIdUniquenessChecker userAuthIdUniquenessChecker)
        {
            this._next = next;
            _treesContext = treesContext;
            _userAuthIdUniquenessChecker = userAuthIdUniquenessChecker;
        }

        public async Task Invoke(HttpContext context)
        {
            await GenerateOrUpdateDomainUserProfile(context);

            await _next.Invoke(context);
        }

        private class UserMailPoco
        {
            public string UserAuthId { get; set; }
            public string ContactEmailAddress { get; set; }
        }
        private async Task GenerateOrUpdateDomainUserProfile(HttpContext context)
        {
            var mail = context.GetUserEmail();
            var userAuthId = context.GetFirebaseUserAuthId();

            if (!string.IsNullOrEmpty(userAuthId))
            {

                var user =
                    await _treesContext.Users.FirstOrDefaultAsync(u => u.UserAuthId == userAuthId);

                if (user is null)
                {
                    user = UserProfile.CreateUserProfile(userAuthId, new MailAddress(mail), null, null, null,
                        _userAuthIdUniquenessChecker);

                    await _treesContext.Users.AddAsync(user);
                }
                else if (!Equals(user.ContactEmailAddress?.Address, mail))
                {
                    user.UpdateContactEmailAddress(new MailAddress(mail));
                }

                await _treesContext.SaveChangesAsync();
            }
        }
    }
}
