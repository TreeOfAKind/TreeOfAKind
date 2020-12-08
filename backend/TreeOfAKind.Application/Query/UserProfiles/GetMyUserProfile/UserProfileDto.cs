using System;
using NodaTime;

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
            if(birthDate.HasValue) BirthDate = LocalDate.FromDateTime(birthDate.Value);
        }
    }
}
