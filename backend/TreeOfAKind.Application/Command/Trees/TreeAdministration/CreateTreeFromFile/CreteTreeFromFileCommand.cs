using System;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTreeFromFile
{
    public class CreateTreeFromFileCommand : CommandBase<TreeId>
    {
        public string UserAuthId { get; }
        public Document Document { get; }
        public string TreeName { get; }

        public CreateTreeFromFileCommand(string userAuthId, Document document, string treeName)
        {
            Document = document;
            UserAuthId = userAuthId;
            TreeName = treeName;
        }
    }
}
