using System.Collections.Generic;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees.People;
using TreeOfAKind.Domain.Trees.Rules;

namespace TreeOfAKind.Domain.Trees
{
    public class TreeRelations : Entity
    {
        public TreeId TreeId { get; private set; }
        public IReadOnlyCollection<Relation> Relations
            => _relations;

        private readonly List<Relation> _relations 
            = new List<Relation>();

        private TreeRelations()
        {
            TreeId = default!;
        }

        public TreeRelations(TreeId treeId)
        {
            TreeId = treeId;
        }

        public void AddRelation(PersonId from, PersonId to, RelationType relationType)
        {
            CheckRule(new ThereIsNoPreviousRelationBetweenRule(Relations, from,to));
            _relations.Add(new Relation(from,to,relationType));
            if (relationType == RelationType.Spouse) _relations.Add(new Relation(to,from,relationType));
            CheckRule(new ThereIdNoCyclesInRelationsRule(Relations));
        }

        public void RemoveRelation(PersonId from, PersonId to)
        {
            _relations.RemoveAll(r => r.From == from && r.To == to);
            _relations.RemoveAll(r => r.From == to && r.To == from);
        }
    }
}