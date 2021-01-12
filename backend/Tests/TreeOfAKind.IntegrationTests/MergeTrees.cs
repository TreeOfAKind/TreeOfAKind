using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeOfAKind.Application.Command.Trees.People;
using TreeOfAKind.Application.Command.Trees.People.AddPerson;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.MergeTrees;
using TreeOfAKind.Application.Query.Trees.GetTree;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using TreeOfAKind.Infrastructure.Processing;
using Xunit;
using Relation = TreeOfAKind.Application.Command.Trees.People.Relation;

namespace TreeOfAKind.IntegrationTests
{
    public class MergeTrees : TreeIntegrationTestsBase
    {
        public MergeTrees(ApplicationFixture applicationFixture) : base(applicationFixture)
        {
        }

        [Fact]
        public async Task MergeTrees_TreesWithOneOverlappingPerson_PeopleAndRelationsAreMerged()
        {
            var treeId = await CreateTree();

            var queenId = await CommandsExecutor.Execute(
                new AddPersonCommand(
                    AuthId,
                    treeId,
                    "Elżbieta",
                    "II",
                    Gender.Female,
                    new DateTime(1926, 4, 21),
                    null,
                    "Queen",
                    "Some biography"));


            var princeId = await CommandsExecutor.Execute(
                new AddPersonCommand(
                    AuthId,
                    treeId,
                    "Filip",
                    null,
                    Gender.Male,
                    new DateTime(1921, 5, 10),
                    null,
                    "Prince",
                    "Some biography of Filip",
                    new List<Relation>
                    {
                        new Relation(queenId, RelationDirection.FromAddedPerson, RelationType.Spouse)
                    }));

            var secondTreeId = await CreateTree();

            var queenInSecondTreeId = await CommandsExecutor.Execute(
                new AddPersonCommand(
                    AuthId,
                    secondTreeId,
                    "Elżbieta",
                    "II",
                    Gender.Female,
                    new DateTime(1926, 4, 21),
                    null,
                    "Queen",
                    "Some biography"));

            var queenMotherInSecondTree = await CommandsExecutor.Execute(
                new AddPersonCommand(
                    AuthId,
                    secondTreeId,
                    "Mother",
                    "Of Queen",
                    Gender.Female,
                    new DateTime(1896, 4, 21),
                    null,
                    "",
                    "Some biography",
                    new List<Relation>
                    {
                        new Relation(queenInSecondTreeId, RelationDirection.ToAddedPerson, RelationType.Mother)
                    }));

            var mergedTreeId = await CommandsExecutor.Execute(
                new MergeTreesCommand(treeId, secondTreeId, AuthId));

            var mergedTree = await QueriesExecutor.Execute(
                new GetTreeQuery(AuthId, mergedTreeId));

            Assert.Equal(3, mergedTree.People.Count);
            Assert.NotNull(mergedTree.People.FirstOrDefault(p => p.Name == "Elżbieta")?.Mother);
            Assert.NotNull(mergedTree.People.FirstOrDefault(p => p.Name == "Elżbieta")?.Spouse);
        }
    }
}
