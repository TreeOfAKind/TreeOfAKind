using System;
using System.Net.Mail;
using NodaTime;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Query.UserProfiles.GetMyUserProfile
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public LocalDate? BirthDate { get; set; }
        public string MailAddress { get; set; }

        public UserProfileDto(Guid id, string firstName, string lastName, DateTime? birthDate, MailAddress mailAddress)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate?.GetDate();
            MailAddress = mailAddress?.Address;
        }

        public UserProfileDto(UserProfile userProfile)
        {
            Id = userProfile.Id.Value;
            FirstName = userProfile.FirstName;
            LastName = userProfile.LastName;
            BirthDate = userProfile.BirthDate?.GetDate();
            MailAddress = userProfile.ContactEmailAddress?.Address;
        }
    }
}
