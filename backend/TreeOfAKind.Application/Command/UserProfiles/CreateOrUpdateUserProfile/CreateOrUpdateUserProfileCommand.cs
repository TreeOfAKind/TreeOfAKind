using System;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Command.UserProfiles.CreateOrUpdateUserProfile
{
    public class CreateOrUpdateUserProfileCommand : CommandBase<UserId>
    {
        public string UserAuthId { get; }
        public string? FirstName { get; }
        public string? LastName { get; }
        public DateTime? BirthDate { get; }

        public CreateOrUpdateUserProfileCommand(string userAuthId, string? firstName, string? lastName, DateTime? birthDate)
        {
            UserAuthId = userAuthId;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }
    }
}