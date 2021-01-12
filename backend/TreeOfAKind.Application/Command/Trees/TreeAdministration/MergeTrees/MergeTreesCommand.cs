using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.MergeTrees
{
    public class MergeTreesCommand : CommandBase<TreeId>
    {
        public TreeId First { get; }
        public TreeId Second { get; }
        public string RequesterUserAuthId { get; }

        public MergeTreesCommand(TreeId first, TreeId second, string requesterUserAuthId)
        {
            First = first;
            Second = second;
            RequesterUserAuthId = requesterUserAuthId;
        }
    }
}
