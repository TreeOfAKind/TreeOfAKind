using System.Collections.Generic;
using System.Linq;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Domain.Trees.Rules
{
    public class TreeMustContainPersonRule : IBusinessRule
    {
        public IReadOnlyCollection<Person> People { get; }
        public PersonId PersonId { get; }
        public TreeMustContainPersonRule(IReadOnlyCollection<Person> people, PersonId personId)
        {
            People = people;
            PersonId = personId;
        }

        public bool IsBroken()
            => People.All(p => p.Id != PersonId);

        public string Message => "Operations on a tree must affect only people that belong to this tree.";
    }
}