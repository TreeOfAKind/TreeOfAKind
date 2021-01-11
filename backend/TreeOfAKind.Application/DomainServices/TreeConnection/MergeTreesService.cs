using System;
using System.Collections.Generic;
using System.Linq;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.DomainServices.TreeConnection
{
    public class MergeTreesService : IMergeTreesService
    {
        private readonly IMergePeopleService _mergePeopleService;

        public MergeTreesService(IMergePeopleService mergePeopleService)
        {
            _mergePeopleService = mergePeopleService;
        }

        public Tree Merge(Tree first, Tree second, UserId userId)
        {
            var peopleFromFirstTree = first.People;
            var peopleFromSecondTree = new List<Person>(second.People);

            var idMapping = new Dictionary<PersonId, PersonId>();

            var name = "Merged " + first.Name + " and " + second.Name;

            if (name.Length > StringLengths.Long)
            {
                name = name.Remove(StringLengths.Short);
            }

            var newTree = Tree.CreateNewTree(name, userId);
            foreach (var personFromFirstTree in peopleFromFirstTree)
            {
                var similarPerson = peopleFromSecondTree.FirstOrDefault(p =>
                    p.Name == personFromFirstTree.Name && p.LastName == personFromFirstTree.LastName);

                peopleFromSecondTree.Remove(similarPerson);

                var person = _mergePeopleService.Merge(personFromFirstTree, similarPerson, newTree);

                idMapping.Add(personFromFirstTree.Id, person.Id);
                if (similarPerson is not null) idMapping.Add(similarPerson.Id, person.Id);
            }

            foreach (var personFromSecondTree in peopleFromSecondTree)
            {
                var person = _mergePeopleService.Merge(personFromSecondTree, null, newTree);
                idMapping.Add(personFromSecondTree.Id, person.Id);
            }

            AddRelationsToTree(first.TreeRelations.Relations, newTree, idMapping);

            AddRelationsToTree(second.TreeRelations.Relations, newTree, idMapping);

            return newTree;
        }

        private static void AddRelationsToTree(IEnumerable<Relation> relations, Tree newTree,
            Dictionary<PersonId, PersonId> idMapping)
        {
            foreach (var relation in relations)
            {
                try
                {
                    newTree.AddRelation(idMapping[relation.From], idMapping[relation.To], relation.RelationType);
                }
                catch (BusinessRuleValidationException)
                {
                }
            }
        }
    }
}
