﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.RemoveTreeOwner
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

            if (!tree.TreeOwners.Any())
            {
                await _treeRepository.RemoveAsync(tree, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
