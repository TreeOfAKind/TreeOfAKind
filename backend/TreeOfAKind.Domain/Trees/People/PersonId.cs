using System;
using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.Trees.People
{
    public class PersonId : TypedIdValueBase
    {
        public PersonId(Guid value) : base(value)
        {
        }
    }
}