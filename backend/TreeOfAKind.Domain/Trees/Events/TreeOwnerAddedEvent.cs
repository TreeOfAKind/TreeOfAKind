using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Domain.Trees.Events
{
    public class TreeOwnerAddedEvent : DomainEventBase
    {
        public TreeId Tree { get; }
        public UserProfile Invited { get; }
        public UserProfile Invitor { get; }
        public TreeOwnerAddedEvent(TreeId tree, UserProfile invited, UserProfile invitor)
        {
            Tree = tree;
            Invited = invited;
            Invitor = invitor;
        }
    }
}
