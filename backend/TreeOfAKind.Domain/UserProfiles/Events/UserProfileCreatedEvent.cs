using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.UserProfiles.Events
{
    public class UserProfileCreatedEvent : DomainEventBase
    {
        public UserId UserId { get; }
        
        public UserProfileCreatedEvent(UserId userId)
        {
            UserId = userId;
        }
    }
}