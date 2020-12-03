using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.CreateTree
{
    public class CreateTreeCommand : CommandBase<TreeId>
    {
        public string TreeName { get; }
        public string UserAuthId { get; }

        public CreateTreeCommand(string treeName, string userAuthId)
        {
            TreeName = treeName;
            UserAuthId = userAuthId;
        }
    }
}