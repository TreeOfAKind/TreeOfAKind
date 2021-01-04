using System;
using System.Data;
using System.Net.Mail;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using TreeOfAKind.API.SeedWork;
using TreeOfAKind.Application.Configuration.Data;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.API.Configuration
{
    public class UserProfileMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        private const string QueryUserProfile =
            "SELECT TOP 1 up.ContactEmailAddress FROM [trees].[UserProfiles] up WHERE up.[UserAuthId] = @UserAuthId";

        private const string InsertUserProfile =
            "INSERT INTO [trees].[UserProfiles] (Id, UserAuthId, ContactEmailAddress) VALUES (@Id, @UserAuthId, @ContactEmailAddress)";

        private const string UpdateUserProfile =
            "UPDATE [trees].[UserProfiles] SET ContactEmailAddress=@ContactEmailAddress WHERE UserAuthId=@UserAuthId";

        public UserProfileMiddleware(
            RequestDelegate next, ISqlConnectionFactory sqlConnectionFactory)
        {
            this._next = next;
            _sqlConnectionFactory = sqlConnectionFactory;
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
                var connection = _sqlConnectionFactory.GetOpenConnection();

                using var transaction = connection.BeginTransaction();
                var user =
                    await connection.QueryFirstOrDefaultAsync<UserMailPoco>(QueryUserProfile, new {userAuthId}, transaction);

                if (user is null)
                {
                    await CreateUserProfile(connection, userAuthId, mail, transaction);
                }
                else if (!Equals(user.ContactEmailAddress, mail))
                {
                    await UpdateContactEmail(connection, mail, userAuthId, transaction);
                }

                transaction.Commit();
            }
        }

        private static async Task UpdateContactEmail(IDbConnection connection, string mail, string userAuthId,
            IDbTransaction transaction)
        {
            await connection.ExecuteAsync(UpdateUserProfile,
                new {ContactEmailAddress = mail, UserAuthId = userAuthId}, transaction);
        }

        private static async Task CreateUserProfile(IDbConnection connection, string userAuthId, string mail,
            IDbTransaction transaction)
        {
            await connection.ExecuteAsync(InsertUserProfile,
                new {Id = Guid.NewGuid(), UserAuthId = userAuthId, ContactEmailAddress = mail}, transaction);
        }
    }
}
