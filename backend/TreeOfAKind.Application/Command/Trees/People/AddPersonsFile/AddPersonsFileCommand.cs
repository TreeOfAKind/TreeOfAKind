using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Command.Trees.People.AddPersonsFile
{
    public class AddPersonsFileCommand : TreeOperationCommandBase<File>
    {
        public Document Document { get; }
        public PersonId PersonId { get; }

        public AddPersonsFileCommand(string requesterUserAuthId, TreeId treeId, Document document, PersonId personId) : base(requesterUserAuthId, treeId)
        {
            Document = document;
            PersonId = personId;
        }
    }
}
