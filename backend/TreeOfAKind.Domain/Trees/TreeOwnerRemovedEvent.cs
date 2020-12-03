using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Domain.Trees
{
    public class TreeOwnerRemovedEvent : DomainEventBase
    {
        private TreeId Tree { get; }
        public UserId Id { get; }
        public TreeOwnerRemovedEvent(TreeId tree, UserId id)
        {
            Tree = tree;
            Id = id;
        }
    }
}