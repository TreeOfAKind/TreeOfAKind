using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.UserProfiles
{
    public class UserProfileUpdatedEvent : DomainEventBase
    {
        public UserId Id { get; }
        public UserProfileUpdatedEvent(UserId id)
        {
            Id = id;
        }
    }
}