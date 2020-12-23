using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Command.Trees.People.AddRelation
{
    public class AddRelationCommand : TreeOperationCommandBase
    {
        public PersonId From { get; }
        public PersonId To { get; }
        public RelationType RelationType { get; }

        public AddRelationCommand(string requesterUserAuthId, TreeId treeId, PersonId from, PersonId to,
            RelationType relationType) : base(requesterUserAuthId, treeId)
        {
            From = from;
            To = to;
            RelationType = relationType;
        }
    }
}
