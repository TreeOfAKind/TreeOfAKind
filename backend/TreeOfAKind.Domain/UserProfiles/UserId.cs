using System;
using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.UserProfiles
{
    public class UserId : TypedIdValueBase
    {
        public UserId(Guid value) : base(value)
        {
        }
    }
}