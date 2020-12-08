using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Command.Trees.People.RemovePerson
{
    public class RemovePersonCommand : TreeOperationCommandBase
    {
        public PersonId PersonId { get; }
        public RemovePersonCommand(string requesterUserAuthId, TreeId treeId, PersonId personId) : base(requesterUserAuthId, treeId)
        {
            PersonId = personId;
        }
    }
}
