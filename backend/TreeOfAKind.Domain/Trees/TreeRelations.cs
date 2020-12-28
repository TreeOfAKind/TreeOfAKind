using System.Collections.Generic;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees.People;
using TreeOfAKind.Domain.Trees.Rules;

namespace TreeOfAKind.Domain.Trees
{
    public class TreeRelations : Entity
    {
        public IReadOnlyCollection<Relation> Relations
            => _relations;

        private readonly List<Relation> _relations
            = new List<Relation>();

        public TreeRelations()
        {
        }

        public void AddRelation(PersonId from, PersonId to, RelationType relationType)
        {
            var relation = new Relation(from, to, relationType);
            if (_relations.Contains(relation)) return;

            CheckRule(new ThereIsNoExistingRelationBetweenRule(Relations, from, to));
            CheckRule(new ThereAreOnlyOneParentOfEachGender(Relations, from, to, relationType));

            _relations.Add(relation);
            if (relationType == RelationType.Spouse) _relations.Add(new Relation(to,from,relationType));

            CheckRule(new ThereAreNoCyclesInRelationsRule(Relations));
        }

        public void RemoveRelation(PersonId from, PersonId to)
        {
            _relations.RemoveAll(r => r.From == from && r.To == to);
            _relations.RemoveAll(r => r.From == to && r.To == from);
        }

        public void RemoveAllPersonRelations(PersonId personId)
        {
            _relations.RemoveAll(r => r.From == personId || r.To == personId);
        }

        public void RemoveFromPersonRelations(PersonId personId)
        {
            _relations.RemoveAll(r => r.From == personId);
            _relations.RemoveAll(r => r.To == personId && r.RelationType == RelationType.Spouse);
        }
    }
}
