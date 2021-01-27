using System;
using System.Net.Mail;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Command.UserProfiles.CreateOrUpdateUserProfile
{
    public class CreateOrUpdateUserProfileCommand : CommandBase<UserId>
    {
        public string UserAuthId { get; }
        public MailAddress MailAddress { get; }
        public string? FirstName { get; }
        public string? LastName { get; }
        public DateTime? BirthDate { get; }

        public CreateOrUpdateUserProfileCommand(string userAuthId, MailAddress mailAddress, string? firstName, string? lastName, DateTime? birthDate)
        {
            UserAuthId = userAuthId;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            MailAddress = mailAddress;
        }
    }
}
