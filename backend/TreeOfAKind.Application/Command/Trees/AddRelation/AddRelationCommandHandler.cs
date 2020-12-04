using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.AddRelation
{
    public class AddRelationCommandHandler : ICommandHandler<AddRelationCommand, Unit>
    {
        private readonly ITreeRepository _treeRepository;

        public AddRelationCommandHandler(ITreeRepository treeRepository)
        {
            _treeRepository = treeRepository;
        }

        public async Task<Unit> Handle(AddRelationCommand request, CancellationToken cancellationToken)
        {
            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);
            
            tree!.AddRelation(request.From, request.To, request.RelationType);
            
            return Unit.Value;
        }
    }
}