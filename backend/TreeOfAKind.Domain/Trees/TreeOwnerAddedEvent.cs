using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Domain.Trees
{
    public class TreeOwnerAddedEvent : DomainEventBase
    {
        public Tree Tree { get; }
        public UserProfile UserProfile { get; }
        public TreeOwnerAddedEvent(Tree tree, UserProfile userProfile)
        {
            Tree = tree;
            UserProfile = userProfile;
        }
    }
}