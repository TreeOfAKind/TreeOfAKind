using System;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Command.UserProfiles.CreateOrUpdateUserProfile
{
    public class CreateOrUpdateUserProfileCommand : CommandBase<UserId>
    {
        public string AuthUserId { get; }
        public string? FirstName { get; }
        public string? LastName { get; }
        public DateTime? BirthDate { get; }

        public CreateOrUpdateUserProfileCommand(string authUserId, string? firstName, string? lastName, DateTime? birthDate)
        {
            AuthUserId = authUserId;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }
    }
}