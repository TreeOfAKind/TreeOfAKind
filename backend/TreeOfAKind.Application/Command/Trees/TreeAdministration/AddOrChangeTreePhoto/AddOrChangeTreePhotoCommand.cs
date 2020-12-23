using System;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.AddOrChangeTreePhoto
{
    public class AddOrChangeTreePhotoCommand : TreeOperationCommandBase<Uri>
    {
        public Document Document { get; }

        public AddOrChangeTreePhotoCommand(string requesterUserAuthId, TreeId treeId, Document document) : base(requesterUserAuthId, treeId)
        {
            Document = document;
        }
    }
}
