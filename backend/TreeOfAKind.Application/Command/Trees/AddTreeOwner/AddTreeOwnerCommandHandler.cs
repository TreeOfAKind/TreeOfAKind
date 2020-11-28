using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Application.Services;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Domain.UserProfiles.Rules;

namespace TreeOfAKind.Application.Command.Trees.AddTreeOwner
{
    public class AddTreeOwnerCommandHandler : ICommandHandler<AddTreeOwnerCommand>
    {
        private readonly ITreeRepository _treeRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserAuthIdProvider _userAuthIdProvider;
        private readonly IAuthUserIdUniquenessChecker _authUserIdUniquenessChecker;

        public AddTreeOwnerCommandHandler(ITreeRepository treeRepository, IUserProfileRepository userProfileRepository,
            IUserAuthIdProvider userAuthIdProvider, IAuthUserIdUniquenessChecker authUserIdUniquenessChecker)
        {
            _treeRepository = treeRepository;
            _userProfileRepository = userProfileRepository;
            _userAuthIdProvider = userAuthIdProvider;
            _authUserIdUniquenessChecker = authUserIdUniquenessChecker;
        }

        public async Task<Unit> Handle(AddTreeOwnerCommand request, CancellationToken cancellationToken)
        {
            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);

            var authId = await _userAuthIdProvider.GetUserAuthId(request.AddedPersonAddress);
            var addedUserProfile = await _userProfileRepository.GetByAuthUserIdAsync(authId);

            if (addedUserProfile is null)
            {
                addedUserProfile = UserProfile.CreateUserProfile(
                    authId, null, null, null, _authUserIdUniquenessChecker);

                await _userProfileRepository.AddAsync(addedUserProfile, cancellationToken);
            }

            tree.AddTreeOwner(addedUserProfile);
            
            return Unit.Value;
        }
    }
}