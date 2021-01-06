using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreeOfAKind.Application.Command;
using TreeOfAKind.Application.Command.Trees.People;
using TreeOfAKind.Application.Command.Trees.People.AddPerson;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTreeFromFile;
using TreeOfAKind.Application.Query.Trees.GetTree;
using TreeOfAKind.Application.Query.Trees.GetTreeFileExport;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using TreeOfAKind.Infrastructure.Processing;
using Xunit;
using Relation = TreeOfAKind.Application.Command.Trees.People.Relation;

namespace TreeOfAKind.IntegrationTests
{
    public class ExportImport : TreeIntegrationTestsBase
    {
        public ExportImport(ApplicationFixture applicationFixture) : base(applicationFixture)
        {
        }

        private async Task<TreeDto> PrepareTreeWithTwoPeople()
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

            return await QueriesExecutor.Execute(
                new GetTreeQuery(AuthId, treeId));
        }

        [Fact]
        public async Task FileExportFileImport_TreeWithTwoPeople_PeoplePropertiesAreImported()
        {
            var tree = await PrepareTreeWithTwoPeople();

            var export =
                await QueriesExecutor.Execute(new GetTreeFileExportQuery(AuthId, new TreeId(tree.TreeId)));

            const string createdTreeName = "Drzewko 🍑";
            var document = new Document(export, "text/xml", "Drzewko");
            var importTreeId = await CommandsExecutor.Execute(new CreateTreeFromFileCommand(AuthId, document,createdTreeName));

            var importedTree = await QueriesExecutor.Execute(new GetTreeQuery(AuthId, importTreeId));


            Assert.Equal(createdTreeName, importedTree.TreeName);

            var princeExpected = tree.People.First(p => p.Gender == Gender.Male);
            var queenExpected = tree.People.First(p => p.Gender == Gender.Female);
            var prince = tree.People.First(p => p.Gender == Gender.Male);
            var queen = tree.People.First(p => p.Gender == Gender.Female);

            AssertPersonFieldsEqual(princeExpected, prince);
            AssertPersonFieldsEqual(queenExpected, queen);

            Assert.Equal(queen.Id, prince.Spouse);
            Assert.Equal(prince.Id, queen.Spouse);
        }

        private static void AssertPersonFieldsEqual(PersonDto expected, PersonDto actual)
        {
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.LastName, actual.LastName);
            Assert.Equal(expected.Gender, actual.Gender);
            Assert.Equal(expected.Biography, actual.Biography);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.BirthDate, actual.BirthDate);
            Assert.Equal(expected.DeathDate, actual.DeathDate);
            Assert.Equal(expected.Children, actual.Children);
            Assert.Equal(expected.Mother, actual.Mother);
            Assert.Equal(expected.Father, actual.Father);
        }
    }
}
