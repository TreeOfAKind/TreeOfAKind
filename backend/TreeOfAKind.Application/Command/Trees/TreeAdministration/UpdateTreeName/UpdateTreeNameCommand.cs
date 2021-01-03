using MediatR;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.UpdateTreeName
{
    public class UpdateTreeNameCommand : TreeOperationCommandBase<Unit>
    {
        public string TreeName { get; }

        public UpdateTreeNameCommand(string requesterUserAuthId, TreeId treeId, string treeName) : base(
            requesterUserAuthId, treeId)
        {
            TreeName = treeName;
        }
    }
}
