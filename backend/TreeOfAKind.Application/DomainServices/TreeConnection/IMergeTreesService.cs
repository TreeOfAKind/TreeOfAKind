using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.DomainServices.TreeConnection
{
    public interface IMergeTreesService
    {
        Tree Merge(Tree first, Tree second, UserId userId);
    }
}
