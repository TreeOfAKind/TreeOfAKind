using System.Net.Mail;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTree
{
    public class CreateTreeCommand : CommandBase<TreeId>
    {
        public string TreeName { get; }
        public string UserAuthId { get; }
        public MailAddress MailAddress { get; }

        public CreateTreeCommand(string treeName, string userAuthId, MailAddress mailAddress)
        {
            TreeName = treeName;
            UserAuthId = userAuthId;
            MailAddress = mailAddress;
        }
    }
}
