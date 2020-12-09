using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NSubstitute;
using TreeOfAKind.Application.Command;
using TreeOfAKind.Application.Command.Trees.People.AddOrChangePersonsPhoto;
using TreeOfAKind.Application.Command.Trees.People.AddPerson;
using TreeOfAKind.Application.Command.Trees.People.AddPersonFile;
using TreeOfAKind.Application.Command.Trees.People.RemovePersonsFile;
using TreeOfAKind.Application.Query.Trees.GetTree;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using TreeOfAKind.Infrastructure.Processing;
using Xunit;

namespace TreeOfAKind.IntegrationTests
{
    public class PersonsFiles : TreeIntegrationTestsBase
    {
        private TreeId _treeId;
        private PersonId _queenId;
        protected readonly Uri _uriExample = new Uri("http://example.com/");

        public PersonsFiles(ApplicationFixture applicationFixture) : base(applicationFixture)
        {
            Init().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private async Task Init()
        {
            _treeId = await CreateTree();

            _queenId = await CommandsExecutor.Execute(
                new AddPersonCommand(
                    AuthId,
                    _treeId,
                    "Elżbieta",
                    "II",
                    Gender.Female,
                    new DateTime(1926, 4, 21),
                    null,
                    "Queen",
                    "Some biography"));
        }

        [Fact]
        private async Task AddPersonsFile_HappyPath_FileIsReturnedInQuery()
        {
            _applicationFixture.FileSaver
                .UploadFile(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<CancellationToken>())
                .Returns(_uriExample);

            var fileId = await CommandsExecutor.Execute(
                new AddPersonsFileCommand(AuthId, _treeId, new Document(Stream.Null, "image/jpg", "file"), _queenId));

            Assert.NotNull(fileId);

            var tree = await QueriesExecutor.Execute(
                new GetTreeQuery(AuthId, _treeId));

            var fileFromQueryId = tree.People.FirstOrDefault()?.Files.FirstOrDefault()?.Id;

            Assert.Equal(fileId.Id.Value, fileFromQueryId);
        }

        [Fact]
        private async Task AddPersonsMainPhoto_HappyPath_FileIsReturnedInQuery()
        {
            _applicationFixture.FileSaver
                .UploadFile(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<CancellationToken>())
                .Returns(_uriExample);

            var fileId = await CommandsExecutor.Execute(
                new AddOrChangePersonsPhotoCommand(AuthId, _treeId, new Document(Stream.Null, "image/jpg", "jeden"), _queenId));

            Assert.NotNull(fileId);

            var tree = await QueriesExecutor.Execute(
                new GetTreeQuery(AuthId, _treeId));

            var fileFromQueryId = tree.People.FirstOrDefault()?.MainPhoto;

            Assert.Equal(fileId.Id.Value, fileFromQueryId?.Id);
            Assert.Equal("jeden", fileFromQueryId?.Name);


            var fileId2 = await CommandsExecutor.Execute(
                new AddOrChangePersonsPhotoCommand(AuthId, _treeId, new Document(Stream.Null, "image/jpg", "dwa"), _queenId));

            var tree2 = await QueriesExecutor.Execute(
                new GetTreeQuery(AuthId, _treeId));

            var fileFromQueryId2 = tree2.People.FirstOrDefault()?.MainPhoto;

            Assert.Equal(fileId2.Id.Value, fileFromQueryId2?.Id);
            Assert.Equal("dwa", fileFromQueryId2?.Name);
        }

        [Fact]
        private async Task RemovePersonsFile_HappyPath_FileIsDeleted()
        {
            _applicationFixture.FileSaver
                .UploadFile(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<CancellationToken>())
                .Returns(_uriExample);

            var file = await CommandsExecutor.Execute(
                new AddPersonsFileCommand(AuthId, _treeId, new Document(Stream.Null, "image/jpg", "file"), _queenId));

            await CommandsExecutor.Execute(
                new RemovePersonsFileCommand(AuthId, _treeId, file.Id, _queenId));

            var tree = await QueriesExecutor.Execute(
                new GetTreeQuery(AuthId, _treeId));

            var files = tree.People.FirstOrDefault()?.Files;

            Assert.NotNull(files);
            Assert.Empty(files);
        }
    }
}
