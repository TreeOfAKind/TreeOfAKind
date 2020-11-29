using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreeOfAKind.Application.Command.Trees.CreateTree;
using TreeOfAKind.Application.Command.UserProfiles.CreateOrUpdateUserProfile;
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
                new CreateOrUpdateUserProfileCommand(AuthId,Name,Surname,BirthDate));

            var treeId = await CommandsExecutor.Execute(
                new CreateTreeCommand(TreeName,AuthId));

            var myTrees = await QueriesExecutor.Execute(
                new GetMyTreesQuery(AuthId));

            Assert.Single(myTrees.Trees);

            var tree = myTrees.Trees[0];
            Assert.Equal(tree.Id, treeId.Value);
            Assert.Equal(TreeName, tree.TreeName);
        }
    }
}