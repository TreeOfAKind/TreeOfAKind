using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Command.Trees.People.AddOrChangePersonsPhoto
{
    public class AddOrChangePersonsPhotoCommand : TreeOperationCommandBase<File>
    {
        public Document Document { get; }
        public PersonId PersonId { get; }

        public AddOrChangePersonsPhotoCommand(string requesterUserAuthId, TreeId treeId, Document document, PersonId personId) : base(requesterUserAuthId, treeId)
        {
            Document = document;
            PersonId = personId;
        }
    }
}
