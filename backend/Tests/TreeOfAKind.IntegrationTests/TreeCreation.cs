using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using TreeOfAKind.Application.Command.Trees.AddTreeOwner;
using TreeOfAKind.Application.Command.Trees.CreateTree;
using TreeOfAKind.Application.Command.Trees.RemoveTreeOwner;
using TreeOfAKind.Application.Command.UserProfiles.CreateOrUpdateUserProfile;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Application.Configuration.Validation;
using TreeOfAKind.Application.Query.Trees.GetMyTrees;
using TreeOfAKind.Infrastructure.Processing;
using Xunit;

namespace TreeOfAKind.IntegrationTests
{
    public class TreeCreation : IClassFixture<ApplicationFixture>
    {
        protected const string TreeName = nameof(TreeCreation) + "Moje super drzewko";
        protected const string AuthId = nameof(TreeCreation);
        protected const string Name = "Bartek";
        protected const string Surname = "Chrostowski";
        protected readonly DateTime BirthDate = new DateTime(1998, 02, 27);
        private readonly ApplicationFixture _applicationFixture;

        public TreeCreation(ApplicationFixture applicationFixture)
        {
            _applicationFixture = applicationFixture;
        }

        [Fact]
        public async Task CreateProfileAndTree_HappyPath_TreeIsCreated()
        {
            var userId = await CommandsExecutor.Execute(
                new CreateOrUpdateUserProfileCommand(AuthId, Name, Surname, BirthDate));

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
        public async Task CreateProfile_TooLongName_ThrowsException()
        {
            await Assert.ThrowsAsync<InvalidCommandException>(async () =>
                await CommandsExecutor.Execute(
                    new CreateOrUpdateUserProfileCommand(AuthId, new string('a', StringLengths.VeryShort + 1), Surname,
                        BirthDate)));
        }

        [Fact]
        public async Task CreateProfileAndTreeAddTreeOwner_HappyPath_AddsTreeOwner()
        {
            var userId = await CommandsExecutor.Execute(
                new CreateOrUpdateUserProfileCommand(AuthId, Name, Surname, BirthDate));

            var treeId = await CommandsExecutor.Execute(
                new CreateTreeCommand(TreeName, AuthId));
            
            var userId2 = await CommandsExecutor.Execute(
                new CreateOrUpdateUserProfileCommand(AuthId + "2", Name + "2", Surname + "2", BirthDate));


            _applicationFixture.UserAuthIdProvider
                .GetUserAuthId(Arg.Any<MailAddress>(), Arg.Any<CancellationToken>())
                .Returns(AuthId + "2");

            await CommandsExecutor.Execute(
                new AddTreeOwnerCommand(treeId, new MailAddress("example@example.com"), AuthId));
            
            await CommandsExecutor.Execute(
                new RemoveTreeOwnerCommand(AuthId + "2", treeId, userId));
        }
    }
}