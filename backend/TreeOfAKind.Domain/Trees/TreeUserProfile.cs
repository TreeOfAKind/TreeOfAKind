using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Domain.Trees
{
    public class TreeUserProfile : ValueObject
    {
        public UserId UserId { get; private set; }
        public TreeId TreeId { get; private set; }

        public TreeUserProfile(UserId userId, TreeId treeId)
        {
            UserId = userId;
            TreeId = treeId;
        }
    }
}
