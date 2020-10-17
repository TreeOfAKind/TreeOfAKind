using System;
using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.Trees
{
    public class TreeId : TypedIdValueBase
    {
        public TreeId(Guid value) : base(value)
        {
        }
    }
}