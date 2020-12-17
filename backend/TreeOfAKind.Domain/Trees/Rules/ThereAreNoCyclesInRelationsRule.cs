using System.Collections.Generic;
using System.Linq;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Domain.Trees.Rules
{
    public class ThereAreNoCyclesInRelationsRule : IBusinessRule
    {
        public IReadOnlyCollection<Relation> Relations { get; }
        public ThereAreNoCyclesInRelationsRule(IReadOnlyCollection<Relation> relations)
        {
            Relations = relations;
        }

        public bool IsBroken()
        {
            var allIdsInChildParentRelation =
                Relations
                    .Select(r => r.From).Concat(
                        Relations.Select(r => r.To))
                    .Distinct();

            var childParent =
                Relations
                    .Where(r =>
                        r.RelationType == RelationType.Father ||
                        r.RelationType == RelationType.Mother)
                    .Select(r => (r.From, r.To))
                    .GroupBy(r => r.From)
                    .ToDictionary(r => r.Key, r => r.ToList().Select(rt => rt.To));

            HashSet<PersonId> allIds = new HashSet<PersonId>(allIdsInChildParentRelation);

            while (allIds.Any())
            {
                HashSet<PersonId> peopleFoundInCurrentSearch = new HashSet<PersonId>();
                var head = allIds.First();

                Queue<PersonId> queue = new Queue<PersonId>();
                queue.Enqueue(head);

                while (queue.Any())
                {
                    var elem = queue.Dequeue();

                    if(!allIds.Remove(elem)) continue;

                    if (!peopleFoundInCurrentSearch.Add(elem)) return true;

                    if(!childParent.ContainsKey(elem)) continue;

                    foreach (var parent in childParent[elem] ?? Enumerable.Empty<PersonId>())
                    {
                        queue.Enqueue(parent);
                    }
                }
            }

            return false;
        }

        public string Message => "Relations must not contain cycle!";
    }
}
