using System;
using System.ComponentModel.DataAnnotations;

namespace TreeOfAKind.API.UserProfiles
{
    public class CreateOrUpdateUserProfileRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)] public DateTime? BirthDate { get; set; }
    }
}
