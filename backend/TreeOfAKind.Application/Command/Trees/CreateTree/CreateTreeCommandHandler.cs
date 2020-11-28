using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Command.Trees.CreateTree
{
    public class CreateTreeCommandHandler : ICommandHandler<CreateTreeCommand, TreeId>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IAuthUserIdUniquenessChecker _authUserIdUniquenessChecker;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTreeCommandHandler(
            IUserProfileRepository userProfileRepository,
            IAuthUserIdUniquenessChecker authUserIdUniquenessChecker,
            IUnitOfWork unitOfWork)
        {
            _userProfileRepository = userProfileRepository;
            _authUserIdUniquenessChecker = authUserIdUniquenessChecker;
            _unitOfWork = unitOfWork;
        }

        public async Task<TreeId> Handle(CreateTreeCommand request, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileRepository.GetByAuthUserIdAsync(request.AuthUserId);

            if (userProfile is null)
            {
                userProfile = UserProfile.CreateUserProfile(
                    request.AuthUserId, null, null, null, _authUserIdUniquenessChecker);

                await _userProfileRepository.AddAsync(userProfile, cancellationToken);
            }

            var createdTree = Tree.CreateNewTree(request.TreeName, userProfile);
            return createdTree.Id;
        }
    }
}