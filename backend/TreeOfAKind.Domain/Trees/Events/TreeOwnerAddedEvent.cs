using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Domain.Trees.Events
{
    public class TreeOwnerAddedEvent : DomainEventBase
    {
        public TreeId Tree { get; }
        public UserId UserProfile { get; }
        public TreeOwnerAddedEvent(TreeId tree, UserId userProfile)
        {
            Tree = tree;
            UserProfile = userProfile;
        }
    }
}