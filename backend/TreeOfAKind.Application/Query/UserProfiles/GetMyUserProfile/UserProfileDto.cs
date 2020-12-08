using System;
using System.ComponentModel.DataAnnotations;

namespace TreeOfAKind.Application.Query.UserProfiles.GetMyUserProfile
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)] public DateTime? BirthDate { get; set; }
    }
}
