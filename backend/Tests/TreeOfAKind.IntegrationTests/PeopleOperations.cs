using System;
using System.Threading.Tasks;
using TreeOfAKind.Application.Command.Trees.AddPerson;
using TreeOfAKind.Application.Command.Trees.AddRelation;
using TreeOfAKind.Application.Command.Trees.CreateTree;
using TreeOfAKind.Application.Command.UserProfiles.CreateOrUpdateUserProfile;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using TreeOfAKind.Infrastructure.Processing;
using Xunit;

namespace TreeOfAKind.IntegrationTests
{
    public class PeopleOperations : IClassFixture<ApplicationFixture>
    {
        protected const string TreeName = nameof(PeopleOperations) + "Moje super drzewko";
        protected string AuthId { get; }
        protected const string Name = "Bartek";
        protected const string Surname = "Chrostowski";
        protected readonly DateTime BirthDate = new DateTime(1998, 02, 27);
        private readonly ApplicationFixture _applicationFixture;

        public PeopleOperations(ApplicationFixture applicationFixture)
        {
            _applicationFixture = applicationFixture;
            AuthId = Guid.NewGuid().ToString();
        }

        private async Task<TreeId> CreateTree()
        {
            var userId = await CommandsExecutor.Execute(
                new CreateOrUpdateUserProfileCommand(AuthId, Name, Surname, BirthDate));

            return await CommandsExecutor.Execute(
                new CreateTreeCommand(TreeName, AuthId));
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
                    "Some biography of Filip"));

            await CommandsExecutor.Execute(
                new AddRelationCommand(AuthId, treeId, princeId, queenId, RelationType.Spouse));
        }
    }
}
