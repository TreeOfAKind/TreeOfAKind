using System.Net.Mail;
using MediatR;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.AddTreeOwner
{
    public class AddTreeOwnerCommand : TreeOperationCommandBase
    {
        public string AddedPersonMailAddress { get; }

        public AddTreeOwnerCommand(string requesterUserAuthId, TreeId treeId, string addedPersonMailAddress)
            : base(requesterUserAuthId, treeId)
        {
            AddedPersonMailAddress = addedPersonMailAddress;
        }
    }
}
