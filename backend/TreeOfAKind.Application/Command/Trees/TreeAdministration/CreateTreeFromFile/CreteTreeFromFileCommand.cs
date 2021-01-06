using System;
using System.Net.Mail;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTreeFromFile
{
    public class CreateTreeFromFileCommand : CommandBase<TreeId>
    {
        public string UserAuthId { get; }
        public MailAddress MailAddress { get; }
        public Document Document { get; }
        public string TreeName { get; }

        public CreateTreeFromFileCommand(string userAuthId, MailAddress mailAddress, Document document, string treeName)
        {
            Document = document;
            UserAuthId = userAuthId;
            MailAddress = mailAddress;
            TreeName = treeName;
        }
    }
}
