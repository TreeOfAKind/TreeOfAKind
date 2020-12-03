using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Command.Trees.RemoveTreeOwner
{
    public class RemoveTreeOwnerCommandHandler : ICommandHandler<RemoveTreeOwnerCommand, Unit>
    {
        private readonly ITreeRepository _treeRepository;

        public RemoveTreeOwnerCommandHandler(ITreeRepository treeRepository)
        {
            _treeRepository = treeRepository;
        }

        public async Task<Unit> Handle(RemoveTreeOwnerCommand request, CancellationToken cancellationToken)
        {
            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);
            tree!.RemoveTreeOwner(request.UserToRemoveId);
            
            return Unit.Value;
        }
    }
}