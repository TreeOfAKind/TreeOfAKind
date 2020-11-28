using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles.Events;
using TreeOfAKind.Domain.UserProfiles.Rules;

namespace TreeOfAKind.Domain.UserProfiles
{
    public class UserProfile : Entity, IAggregateRoot
    {
        public UserId Id { get; private set; }
        public string AuthUserId { get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; } 
        public DateTime? BirthDate { get; private set; }
        public IReadOnlyCollection<Tree> OwnedTrees =>
            _ownedTrees.AsReadOnly();
        
        private List<Tree> _ownedTrees = new List<Tree>();
        
        private UserProfile()
        {
            Id = default!;
            AuthUserId = default!;
            FirstName = default!;
            LastName = default!;
            BirthDate = default!;
        }

        private UserProfile(string authUserId, string? firstName, string? lastName,DateTime? birthDate)
        {
            Id = new UserId(Guid.NewGuid());
            AuthUserId = authUserId;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            
            AddDomainEvent(new UserProfileCreatedEvent(Id));
        }

        public static UserProfile CreateUserProfile(
            string authUserId,
            string? firstName,
            string? lastName,
            DateTime? birthDate,
            IAuthUserIdUniquenessChecker authUserIdUniquenessChecker)
        {
            CheckRule(new OnlyAuthorizedUserCanCreateUserProfileRule(authUserId));
            CheckRule(new AuthUserIdMustBeUniqueRule(authUserId, authUserIdUniquenessChecker));

            return new UserProfile(authUserId, firstName, lastName, birthDate);
        }

        public void UpdateUserProfile(string? firstName, string? lastName, DateTime? birthDate)
        {
            AddDomainEvent(new UserProfileUpdatedEvent(Id));
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }
    }
}