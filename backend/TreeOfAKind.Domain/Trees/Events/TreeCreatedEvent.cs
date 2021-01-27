using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.Trees.Events
{
    internal class TreeCreatedEvent : DomainEventBase
    {
        public TreeId Id { get; }
        public TreeCreatedEvent(TreeId id)
        {
            Id = id;
        }
    }
}