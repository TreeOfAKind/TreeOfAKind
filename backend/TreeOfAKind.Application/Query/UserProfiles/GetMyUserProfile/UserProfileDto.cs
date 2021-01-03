using System;
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

        public UserProfileDto(Guid id, string firstName, string lastName, DateTime? birthDate)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate?.GetDate();
        }

        public UserProfileDto(UserProfile userProfile)
        {
            Id = userProfile.Id.Value;
            FirstName = userProfile.FirstName;
            LastName = userProfile.LastName;
            BirthDate = userProfile.BirthDate?.GetDate();
        }
    }
}
