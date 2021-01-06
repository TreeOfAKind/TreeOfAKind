using System;
using System.Net.Mail;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.UserProfiles.Events;
using TreeOfAKind.Domain.UserProfiles.Rules;

namespace TreeOfAKind.Domain.UserProfiles
{
    public class UserProfile : Entity, IAggregateRoot
    {
        public UserId Id { get; private set; }
        public string UserAuthId { get; private set; }
        public MailAddress? ContactEmailAddress { get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public DateTime? BirthDate { get; private set; }

        private UserProfile()
        {
            Id = default!;
            UserAuthId = default!;
            ContactEmailAddress = default!;
            FirstName = default!;
            LastName = default!;
            BirthDate = default!;
        }

        private UserProfile(string userAuthId, MailAddress contactEmailAddress, string? firstName, string? lastName, DateTime? birthDate)
        {
            Id = new UserId(Guid.NewGuid());
            UserAuthId = userAuthId;
            ContactEmailAddress = contactEmailAddress;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            AddDomainEvent(new UserProfileCreatedEvent(Id));
        }

        public static UserProfile CreateUserProfile(
            string userAuthId,
            MailAddress contactEmailAddress,
            string? firstName,
            string? lastName,
            DateTime? birthDate,
            IUserAuthIdUniquenessChecker userAuthIdUniquenessChecker)
        {
            CheckRule(new UserMustBeRegisteredRule(userAuthId));
            CheckRule(new UserAuthIdMustBeUniqueRule(userAuthId, userAuthIdUniquenessChecker));

            return new UserProfile(userAuthId, contactEmailAddress, firstName, lastName, birthDate);
        }

        public void UpdateUserProfile(MailAddress mailAddress, string? firstName, string? lastName, DateTime? birthDate)
        {
            AddDomainEvent(new UserProfileUpdatedEvent(Id));
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            ContactEmailAddress = mailAddress;
        }
    }
}
