using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Domain.UserProfiles.Rules;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTree
{
    public class CreateTreeCommandHandler : ICommandHandler<CreateTreeCommand, TreeId>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ITreeRepository _treeRepository;
        private readonly IUserAuthIdUniquenessChecker _userAuthIdUniquenessChecker;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTreeCommandHandler(
            IUserProfileRepository userProfileRepository,
            IUserAuthIdUniquenessChecker userAuthIdUniquenessChecker,
            IUnitOfWork unitOfWork,
            ITreeRepository treeRepository)
        {
            _userProfileRepository = userProfileRepository;
            _userAuthIdUniquenessChecker = userAuthIdUniquenessChecker;
            _unitOfWork = unitOfWork;
            _treeRepository = treeRepository;
        }

        public async Task<TreeId> Handle(CreateTreeCommand request, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileRepository.GetByUserAuthIdAsync(request.UserAuthId, cancellationToken);

            if (userProfile is null)
            {
                userProfile = UserProfile.CreateUserProfile(
                    request.UserAuthId, null, null, null, _userAuthIdUniquenessChecker);

                await _userProfileRepository.AddAsync(userProfile, cancellationToken);
            }

            var createdTree = Tree.CreateNewTree(request.TreeName, userProfile.Id);

            await _treeRepository.AddAsync(createdTree, cancellationToken);

            await _unitOfWork.CommitAsync();
            return createdTree.Id;
        }
    }
}