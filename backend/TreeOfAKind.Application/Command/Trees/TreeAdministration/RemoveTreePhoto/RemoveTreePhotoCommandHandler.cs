using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.RemoveTreePhoto
{
    public class RemoveTreePhotoCommandHandler : ICommandHandler<RemoveTreePhotoCommand, Unit>
    {
        private readonly ITreeRepository _treeRepository;

        public RemoveTreePhotoCommandHandler(ITreeRepository treeRepository)
        {
            _treeRepository = treeRepository;
        }

        public async Task<Unit> Handle(RemoveTreePhotoCommand request, CancellationToken cancellationToken)
        {
            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);

            tree!.RemoveTreePhoto();

            return Unit.Value;
        }
    }
}
