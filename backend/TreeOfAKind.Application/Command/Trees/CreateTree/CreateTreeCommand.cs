using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.CreateTree
{
    public class CreateTreeCommand : CommandBase<TreeId>
    {
        public string TreeName { get; }
        public string AuthUserId { get; }

        public CreateTreeCommand(string treeName, string authUserId)
        {
            TreeName = treeName;
            AuthUserId = authUserId;
        }
    }
}