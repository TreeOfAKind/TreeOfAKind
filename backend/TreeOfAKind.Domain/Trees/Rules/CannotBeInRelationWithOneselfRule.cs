using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Domain.Trees.Rules
{
    public class CannotBeInRelationWithOneselfRule : IBusinessRule
    {
        public PersonId From { get; }
        public PersonId To { get; }
        public CannotBeInRelationWithOneselfRule(PersonId from, PersonId to)
        {
            From = from;
            To = to;
        }

        public bool IsBroken()
            => From == To;

        public string Message => "Person cannot be in relation with oneself.";
    }
}