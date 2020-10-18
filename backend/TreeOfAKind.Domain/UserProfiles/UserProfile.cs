using System;
using System.Globalization;
using System.Threading;
using NodaTime;
using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.UserProfiles
{
    public class UserProfile : Entity, IAggregateRoot
    {
        public UserId Id { get; private set; }

        public string AuthUserId { get; private set; }
        public string? Username { get; private set; }
        public string? FirstName { get; private set; }
        
        public string? LastName { get; private set; }

        public DateTime? BirthDate { get; private set; }




        private UserProfile()
        {
            Id = default!;
            AuthUserId = default!;
            Username = default!;
            FirstName = default!;
            LastName = default!;
            BirthDate = default!;
        }

        private UserProfile(string authUserId, string? username, string? firstName, string? lastName,DateTime? birthDate)
        {
            Id = new UserId(new Guid());
            AuthUserId = authUserId;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            
            AddDomainEvent(new UserProfileCreatedEvent(Id));
        }

        public static UserProfile CreateUserProfile(
            string authUserId,
            string username,
            string firstName,
            string lastName,
            DateTime? birthDate,
            IUsernameUniquenessChecker usernameUniquenessChecker,
            IAuthUserIdUniquenessChecker authUserIdUniquenessChecker)
        {
            CheckRule(new OnlyAuthorizedUserCanCreateUserProfileRule(authUserId));
            CheckRule(new AuthUserIdMustBeUniqueRule(authUserId, authUserIdUniquenessChecker));
            CheckRule(new UserNameMustBeUniqueRule(username, usernameUniquenessChecker));

            return new UserProfile(authUserId, username,firstName, lastName, birthDate);
        }
    }
}