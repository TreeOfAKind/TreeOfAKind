using System.Collections.Generic;
using System.Linq;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Domain.Trees.Rules
{
    public class ThereAreOnlyOneSpouseRule : IBusinessRule
    {
        public IReadOnlyCollection<Relation> Relations { get; }
        public PersonId Person1 { get; }
        public PersonId Person2 { get; }
        public RelationType RelationType { get; }
        public ThereAreOnlyOneSpouseRule(IReadOnlyCollection<Relation> relations, PersonId person1, PersonId person2, RelationType relationType)
        {
            Relations = relations;
            Person1 = person1;
            Person2 = person2;
            RelationType = relationType;
        }

        public bool IsBroken()
        {
            if (RelationType != RelationType.Spouse) return false;

            var spouseOfFirstPerson = Relations.FirstOrDefault(r => r.From == Person1);
            var spouseOfSecondPerson = Relations.FirstOrDefault(r => r.From == Person2);

            return spouseOfFirstPerson is not null || spouseOfSecondPerson is not null;
        }

        public string Message => "Person can have only one spouse.";
    }
}
