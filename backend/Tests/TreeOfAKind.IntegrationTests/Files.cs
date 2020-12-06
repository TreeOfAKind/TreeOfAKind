using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using TreeOfAKind.Application.Command;
using TreeOfAKind.Application.Command.Trees.AddOrChangeTreePhoto;
using TreeOfAKind.Application.Query.Trees.GetMyTrees;
using TreeOfAKind.Application.Query.Trees.GetTree;
using TreeOfAKind.Domain.Trees;
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
            var file = new Document(Stream.Null, "image/png");

            _applicationFixture.FileSaver
                .UploadFile(Arg.Any<string>(),Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<CancellationToken>())
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
    }
}
