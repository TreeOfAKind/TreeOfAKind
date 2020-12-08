using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Command.Trees.People.AddOrChangePersonPhoto
{
    public class AddOrChangePersonPhotoCommand : TreeOperationCommandBase<FileId>
    {
        public Document Document { get; }
        public PersonId PersonId { get; }

        public AddOrChangePersonPhotoCommand(string requesterUserAuthId, TreeId treeId, Document document, PersonId personId) : base(requesterUserAuthId, treeId)
        {
            Document = document;
            PersonId = personId;
        }
    }
}
