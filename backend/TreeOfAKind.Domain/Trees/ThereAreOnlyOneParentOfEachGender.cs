using System.Collections.Generic;
using System.Linq;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Domain.Trees
{
    public class ThereAreOnlyOneParentOfEachGender : IBusinessRule
    {
        public IReadOnlyCollection<Relation> Relations { get; }
        public PersonId From { get; }
        public PersonId To { get; }
        public RelationType RelationType { get; }
        public ThereAreOnlyOneParentOfEachGender(IReadOnlyCollection<Relation> relations, PersonId from, PersonId to, RelationType relationType)
        {
            Relations = relations;
            From = from;
            To = to;
            RelationType = relationType;
        }

        public bool IsBroken()
            => (RelationType == RelationType.Father || RelationType == RelationType.Mother)
               && Relations.Contains(new Relation(From, To, RelationType));

        public string Message => "There can only be one mother and one father.";
    }
}
