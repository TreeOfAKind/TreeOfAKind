using System;
using System.Threading.Tasks;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTree;
using TreeOfAKind.Application.Command.UserProfiles.CreateOrUpdateUserProfile;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Infrastructure.Processing;
using Xunit;

namespace TreeOfAKind.IntegrationTests
{
    public abstract class TreeIntegrationTestsBase : IClassFixture<ApplicationFixture>
    {
        protected readonly ApplicationFixture _applicationFixture;

        protected const string TreeName = "Moje super drzewko";
        protected string AuthId { get; }
        protected const string Name = "Bartek";
        protected const string LastName = "Chrostowski";
        protected readonly DateTime BirthDate = new DateTime(1998, 02, 27);

        protected TreeIntegrationTestsBase(ApplicationFixture applicationFixture)
        {
            _applicationFixture = applicationFixture;

            AuthId = Guid.NewGuid().ToString();
        }

        protected async Task<TreeId> CreateTree()
        {
            await CommandsExecutor.Execute(
                new CreateOrUpdateUserProfileCommand(AuthId, Name, LastName, BirthDate));

            return await CommandsExecutor.Execute(
                new CreateTreeCommand(TreeName, AuthId));
        }
    }
}
