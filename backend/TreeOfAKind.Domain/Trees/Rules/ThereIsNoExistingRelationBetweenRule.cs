using System.Collections.Generic;
using System.Linq;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Domain.Trees.Rules
{
    public class ThereIsNoExistingRelationBetweenRule : IBusinessRule
    {
        public IReadOnlyCollection<Relation> Relations { get; }
        public PersonId From { get; }
        public PersonId To { get; }
        public ThereIsNoExistingRelationBetweenRule(IReadOnlyCollection<Relation> relations, PersonId from, PersonId to)
        {
            Relations = relations;
            From = from;
            To = to;
        }

        public bool IsBroken()
            => Relations.Any(r => (r.From == From && r.To == To) || (r.From == To && r.To == From));

        public string Message => "There can only be one type of relation between people.";
    }
}