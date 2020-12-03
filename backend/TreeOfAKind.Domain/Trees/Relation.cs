using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees.People;
using TreeOfAKind.Domain.Trees.Rules;

namespace TreeOfAKind.Domain.Trees
{
    public class Relation : ValueObject
    {
        public PersonId From { get; private set; }
        public PersonId To { get; private set; }
        public RelationType RelationType { get; private set; }

        public Relation()
        {
            From = default!;
            To = default!;
            RelationType = default!;
        }

        public Relation(PersonId from, PersonId to, RelationType relationType)
        {
            CheckRule(new CannotBeInRelationWithOneselfRule(from,to));
            From = from;
            To = to;
            RelationType = relationType;
        }
    }
}