using MediatR;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Command.Trees.RemoveRelation
{
    public class RemoveRelationCommand : TreeOperationCommandBase
    {
        public PersonId First { get; }
        public PersonId Second { get; }

        public RemoveRelationCommand(string requesterUserAuthId, TreeId treeId, PersonId first, PersonId second) : base(
            requesterUserAuthId, treeId)
        {
            First = first;
            Second = second;
        }
    }
}
