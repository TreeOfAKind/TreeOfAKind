using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeOfAKind.Application.Command.Trees.People.AddPerson;
using TreeOfAKind.Application.Command.Trees.People.AddRelation;
using TreeOfAKind.Application.Configuration.Authorization;
using TreeOfAKind.Application.Query.Trees.GetTree;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using TreeOfAKind.Infrastructure.Processing;
using Xunit;

namespace TreeOfAKind.IntegrationTests
{
    public class PeopleOperations : TreeIntegrationTestsBase
    {
        public PeopleOperations(ApplicationFixture applicationFixture) : base(applicationFixture)
        {
        }

        [Fact]
        public async Task AddPersonToTree_HappyPath_PeopleAreAdded()
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
                    new List<AddPersonCommand.Relation>
                    {
                        new AddPersonCommand.Relation(queenId, RelationDirection.FromAddedPerson, RelationType.Spouse)
                    }));

            var tree = await QueriesExecutor.Execute(
                new GetTreeQuery(AuthId, treeId));

            Assert.Equal(2, tree.People.Count);
            Assert.Equal(1, tree.People.FirstOrDefault()?.Spouses.Count);
            Assert.Null(tree.People.FirstOrDefault()?.Mother);
            Assert.Null(tree.People.FirstOrDefault()?.Father);

            Assert.Equal(TreeName, tree.TreeName);
            Assert.Equal(treeId.Value, tree.TreeId);
        }

        [Fact]
        public async Task AddRelation_TwoMothers_ThrowsException()
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

            var queenId2 = await CommandsExecutor.Execute(
                new AddPersonCommand(
                    AuthId,
                    treeId,
                    "Elżbieta",
                    "III",
                    Gender.Female,
                    new DateTime(1926, 4, 21),
                    null,
                    "Queen",
                    "Some biography"));


            var princeId = await CommandsExecutor.Execute(
                new AddPersonCommand(
                    AuthId,
                    treeId,
                    "Queens child",
                    null,
                    Gender.Male,
                    new DateTime(1955, 5, 10),
                    null,
                    "Prince",
                    "Some biography of Filip",
                    null));

            await CommandsExecutor.Execute(
                new AddRelationCommand(AuthId, treeId, princeId, queenId, RelationType.Mother));

            await Assert.ThrowsAsync<BusinessRuleValidationException>(async () =>
                await CommandsExecutor.Execute(
                    new AddRelationCommand(AuthId, treeId, princeId, queenId, RelationType.Mother)));
        }

        [Fact]
        public async Task TreeQuery_UnauthorizedUser_ThrowsException()
        {
            var treeId = await CreateTree();

            await Assert.ThrowsAsync<UnauthorizedException>(async () =>
                await QueriesExecutor.Execute(new GetTreeQuery(AuthId + "2", treeId)));
        }

    }
}
