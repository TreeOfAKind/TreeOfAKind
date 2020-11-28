using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Application.Command.Trees.AddTreeOwner;
using TreeOfAKind.Application.Configuration.Authorization;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Command.Trees
{
    public class AddTreeOwnerCommandAuthorizer : IAuthorizer<TreeOperationCommand>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        public AddTreeOwnerCommandAuthorizer(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<AuthorizationResult> AuthorizeAsync(TreeOperationCommand instance, CancellationToken cancellation = default)
        {
            var user = await _userProfileRepository.GetByUserAuthIdAsync(instance.RequesterUserAuthId);

            return user?.OwnedTrees.Any(t => t.Id == instance.TreeId) == true
                ? AuthorizationResult.Succeed()
                : AuthorizationResult.Fail("User is not owner of this tree");
        }
    }
}