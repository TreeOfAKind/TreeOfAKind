using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.RemoveRelation
{
    public class RemoveRelationCommandHandler : ICommandHandler<RemoveRelationCommand, Unit>
    {
        private readonly ITreeRepository _treeRepository;

        public RemoveRelationCommandHandler(ITreeRepository treeRepository)
        {
            _treeRepository = treeRepository;
        }


        public async Task<Unit> Handle(RemoveRelationCommand request, CancellationToken cancellationToken)
        {
            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);

            tree!.RemoveRelation(request.First, request.Second);

            return Unit.Value;
        }
    }
}
