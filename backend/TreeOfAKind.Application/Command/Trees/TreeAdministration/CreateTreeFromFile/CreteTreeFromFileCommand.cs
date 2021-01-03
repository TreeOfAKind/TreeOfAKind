using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTreeFromFile
{
    public class CreateTreeFromFileCommand : CommandBase<TreeId>
    {
        public Document Document { get; }

        public CreateTreeFromFileCommand(Document document)
        {
            Document = document;
        }
    }
}
