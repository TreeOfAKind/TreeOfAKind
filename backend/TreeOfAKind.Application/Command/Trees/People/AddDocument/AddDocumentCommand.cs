using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Command.Trees.People.AddDocument
{
    public class AddDocumentCommand : TreeOperationCommandBase<FileId>
    {
        public Document Document { get; }
        public PersonId PersonId { get; }

        public AddDocumentCommand(string requesterUserAuthId, TreeId treeId, Document document, PersonId personId) : base(requesterUserAuthId, treeId)
        {
            Document = document;
            PersonId = personId;
        }
    }
}
