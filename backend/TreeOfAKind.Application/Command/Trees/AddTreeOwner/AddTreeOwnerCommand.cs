using System.Net.Mail;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.AddTreeOwner
{
    public class AddTreeOwnerCommand : TreeOperationCommand
    {
        public MailAddress AddedPersonAddress { get; }
        
        public AddTreeOwnerCommand(TreeId treeId, MailAddress addedPersonAddress, string requesterUserAuthId)
            : base(requesterUserAuthId, treeId)
        {
            AddedPersonAddress = addedPersonAddress;
        }
    }
}