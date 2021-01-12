using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Application.DomainServices.TreeConnection;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.MergeTrees
{
    public class MergeTreesCommandHandler : ICommandHandler<MergeTreesCommand, TreeId>
    {
        private readonly IMergeTreesService _mergeTreesService;
        private readonly ITreeRepository _treeRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public MergeTreesCommandHandler(IMergeTreesService mergeTreesService,
            IUserProfileRepository userProfileRepository, ITreeRepository treeRepository)
        {
            _mergeTreesService = mergeTreesService;
            _userProfileRepository = userProfileRepository;
            _treeRepository = treeRepository;
        }

        public async Task<TreeId> Handle(MergeTreesCommand request, CancellationToken cancellationToken)
        {
            var first = await _treeRepository.GetByIdAsync(request.First, cancellationToken);
            var second = await _treeRepository.GetByIdAsync(request.Second, cancellationToken);

            var user =
                await _userProfileRepository.GetByUserAuthIdAsync(request.RequesterUserAuthId, cancellationToken);

            var newTree = _mergeTreesService.Merge(first, second, user!.Id);

            await _treeRepository.AddAsync(newTree, cancellationToken);

            return newTree.Id;
        }
    }
}
