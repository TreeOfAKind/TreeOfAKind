using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.AddTreeOwner;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTree;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.RemoveTreeOwner;
using TreeOfAKind.Application.Command.UserProfiles.CreateOrUpdateUserProfile;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Application.Configuration.Authorization;
using TreeOfAKind.Application.Configuration.Validation;
using TreeOfAKind.Application.Query.Trees.GetMyTrees;
using TreeOfAKind.Infrastructure.Processing;
using Xunit;

namespace TreeOfAKind.IntegrationTests
{
    public class TreeCreation : IClassFixture<ApplicationFixture>
    {
        protected const string TreeName = nameof(TreeCreation) + "Moje super drzewko";
        protected string AuthId { get; }
        protected const string Name = "Bartek";
        protected const string LastName = "Chrostowski";
        protected readonly DateTime BirthDate = new DateTime(1998, 02, 27);
        private readonly ApplicationFixture _applicationFixture;

        public TreeCreation(ApplicationFixture applicationFixture)
        {
            _applicationFixture = applicationFixture;
            AuthId = Guid.NewGuid().ToString();
        }

        [Fact]
        public async Task CreateProfileAndTree_HappyPath_TreeIsCreated()
        {
            var userId = await CommandsExecutor.Execute(
                new CreateOrUpdateUserProfileCommand(AuthId, Name, LastName, BirthDate));

            var treeId = await CommandsExecutor.Execute(
                new CreateTreeCommand(TreeName, AuthId));

            var myTrees = await QueriesExecutor.Execute(
                new GetMyTreesQuery(AuthId));

            Assert.Single(myTrees.Trees);

            var tree = myTrees.Trees[0];
            Assert.Equal(tree.Id, treeId.Value);
            Assert.Equal(TreeName, tree.TreeName);
        }

        [Fact]
        public async Task AddOwner_NotOwnerAdding_ThrowsUnauthorized()
        {
            var userId = await CommandsExecutor.Execute(
                new CreateOrUpdateUserProfileCommand(AuthId, Name, LastName, BirthDate));

            var treeId = await CommandsExecutor.Execute(
                new CreateTreeCommand(TreeName, AuthId));

            _applicationFixture.UserAuthIdProvider
                .GetUserAuthId(Arg.Any<MailAddress>(), Arg.Any<CancellationToken>())
                .Returns(AuthId + "2");

            await Assert.ThrowsAsync<UnauthorizedException>(async () => await CommandsExecutor.Execute(
                new AddTreeOwnerCommand(AuthId + "2", treeId, "example@example.com")));
        }

        [Fact]
        public async Task CreateProfile_TooLongName_ThrowsException()
        {
            await Assert.ThrowsAsync<InvalidCommandException>(async () =>
                await CommandsExecutor.Execute(
                    new CreateOrUpdateUserProfileCommand(AuthId, new string('a', StringLengths.Short + 1), LastName,
                        BirthDate)));
        }

        [Fact]
        public async Task CreateProfileAndTreeAddTreeOwner_HappyPath_AddsTreeOwner()
        {
            var userId = await CommandsExecutor.Execute(
                new CreateOrUpdateUserProfileCommand(AuthId, Name, LastName, BirthDate));

            var treeId = await CommandsExecutor.Execute(
                new CreateTreeCommand(TreeName, AuthId));

            var userId2 = await CommandsExecutor.Execute(
                new CreateOrUpdateUserProfileCommand(AuthId + "2", Name + "2", LastName + "2", BirthDate));


            _applicationFixture.UserAuthIdProvider
                .GetUserAuthId(Arg.Any<MailAddress>(), Arg.Any<CancellationToken>())
                .Returns(AuthId + "2");

            await CommandsExecutor.Execute(
                new AddTreeOwnerCommand(AuthId, treeId, "example@example.com"));

            await CommandsExecutor.Execute(
                new RemoveTreeOwnerCommand(AuthId + "2", treeId, userId));
        }
    }
}
