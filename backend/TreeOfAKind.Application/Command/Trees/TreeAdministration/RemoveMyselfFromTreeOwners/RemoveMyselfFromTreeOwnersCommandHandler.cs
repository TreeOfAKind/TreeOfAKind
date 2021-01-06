using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.RemoveMyselfFromTreeOwners
{
    public class RemoveMyselfFromTreeOwnersCommandHandler : ICommandHandler<RemoveMyselfFromTreeOwnersCommand, Unit>
    {
        private readonly ITreeRepository _treeRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public RemoveMyselfFromTreeOwnersCommandHandler(ITreeRepository treeRepository, IUserProfileRepository userProfileRepository)
        {
            _treeRepository = treeRepository;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<Unit> Handle(RemoveMyselfFromTreeOwnersCommand request, CancellationToken cancellationToken)
        {
            var user = await _userProfileRepository.GetByUserAuthIdAsync(request.RequesterUserAuthId,
                cancellationToken);

            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);

            tree!.RemoveTreeOwner(user!.Id);

            return Unit.Value;
        }
    }
}
