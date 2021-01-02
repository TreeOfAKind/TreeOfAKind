using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Gx.Types;
using NSubstitute;
using TreeOfAKind.Application.Command;
using TreeOfAKind.Application.Command.Trees.People.AddPerson;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.AddOrChangeTreePhoto;
using TreeOfAKind.Application.Query.Trees.GetMyTrees;
using TreeOfAKind.Application.Query.Trees.GetTree;
using TreeOfAKind.Application.Query.Trees.GetTreeFileExport;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using TreeOfAKind.Infrastructure.Processing;
using Xunit;

namespace TreeOfAKind.IntegrationTests
{
    public class Files : TreeIntegrationTestsBase
    {
        protected readonly Uri _uriExample = new Uri("http://example.com/");

        public Files(ApplicationFixture applicationFixture) : base(applicationFixture)
        {
        }

        private async Task<Uri> AddExampleFile(TreeId treeId)
        {
            var file = new Document(Stream.Null, "image/png", "ASDF.png");

            _applicationFixture.FileSaver
                .UploadFile(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Stream>(),
                    Arg.Any<CancellationToken>())
                .Returns(_uriExample);

            return await CommandsExecutor.Execute(
                new AddOrChangeTreePhotoCommand(AuthId, treeId, file));
        }

        [Fact]
        private async Task AddOrCreateTreePhoto_HappyPath_TreePhotoUriIsReturned()
        {
            var treeId = await CreateTree();
            var uri = await AddExampleFile(treeId);

            Assert.Equal(_uriExample, uri);
        }

        [Fact]
        private async Task AddOrCreateTreePhoto_HappyPath_TreePhotoIsReturnedInMyTreesList()
        {
            var treeId = await CreateTree();
            await AddExampleFile(treeId);

            var myTrees = await QueriesExecutor.Execute(
                new GetMyTreesQuery(AuthId));

            Assert.Equal(_uriExample, myTrees.Trees.FirstOrDefault()?.PhotoUri);
        }

        [Fact]
        private async Task AddOrCreateTreePhoto_HappyPath_TreePhotoIsReturnedInTree()
        {
            var treeId = await CreateTree();
            await AddExampleFile(treeId);

            var tree = await QueriesExecutor.Execute(
                new GetTreeQuery(AuthId, treeId));

            Assert.Equal(_uriExample, tree.PhotoUri);
        }

        [Fact]
        private async Task fdsa()
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

            var stream = await QueriesExecutor.Execute(
                new GetTreeFileExportQuery(AuthId, treeId));

            var serializer = new XmlSerializer(typeof(Gx.Gedcomx));

            var gedcom = (Gx.Gedcomx) serializer.Deserialize(stream);


            Assert.NotNull(gedcom);
            Assert.Equal(2, gedcom.Persons.Count);


            Assert.Single(gedcom.Persons,
                p => p.Names.SelectMany(name => name.NameForm.Parts).Any(np => string.Equals(np.Value, "Elżbieta")));

            Assert.NotNull(gedcom.Relationships);

            Assert.Equal(RelationshipType.Couple, gedcom.Relationships.First().KnownType);
        }
    }
}
