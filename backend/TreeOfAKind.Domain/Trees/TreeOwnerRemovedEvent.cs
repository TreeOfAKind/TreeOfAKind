using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Domain.Trees
{
    public class TreeOwnerRemovedEvent : DomainEventBase
    {
        private Tree Tree { get; }
        public UserId Id { get; }
        public TreeOwnerRemovedEvent(Tree tree, UserId id)
        {
            Tree = tree;
            Id = id;
        }
    }
}