using System;
using NodaTime;
using TreeOfAKind.Application.Configuration;

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
    }
}
