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
    public class AddTreeOwnerCommandHandler : ICommandHandler<AddTreeOwnerCommand, Unit>
    {
        private readonly ITreeRepository _treeRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserAuthIdProvider _userAuthIdProvider;
        private readonly IUserAuthIdUniquenessChecker _userAuthIdUniquenessChecker;

        public AddTreeOwnerCommandHandler(ITreeRepository treeRepository, IUserProfileRepository userProfileRepository,
            IUserAuthIdProvider userAuthIdProvider, IUserAuthIdUniquenessChecker userAuthIdUniquenessChecker)
        {
            _treeRepository = treeRepository;
            _userProfileRepository = userProfileRepository;
            _userAuthIdProvider = userAuthIdProvider;
            _userAuthIdUniquenessChecker = userAuthIdUniquenessChecker;
        }

        public async Task<Unit> Handle(AddTreeOwnerCommand request, CancellationToken cancellationToken)
        {
            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);

            var authId = await _userAuthIdProvider.GetUserAuthId(request.AddedPersonAddress);
            var addedUserProfile = await _userProfileRepository.GetByUserAuthIdAsync(authId);

            if (addedUserProfile is null)
            {
                addedUserProfile = UserProfile.CreateUserProfile(
                    authId, null, null, null, _userAuthIdUniquenessChecker);

                await _userProfileRepository.AddAsync(addedUserProfile, cancellationToken);
            }

            tree!.AddTreeOwner(addedUserProfile);
            
            return Unit.Value;
        }
    }
}