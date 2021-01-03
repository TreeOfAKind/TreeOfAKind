using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTree;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Domain.UserProfiles.Rules;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.UpdateTreeName
{
    public class UpdateTreeCommandHandler : ICommandHandler<UpdateTreeNameCommand, Unit>
    {
        private readonly ITreeRepository _treeRepository;

        public UpdateTreeCommandHandler(ITreeRepository treeRepository)
        {
            _treeRepository = treeRepository;
        }

        public async Task<Unit> Handle(UpdateTreeNameCommand request, CancellationToken cancellationToken)
        {
            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);

            tree!.Name = request.TreeName;

            return Unit.Value;
        }
    }
}
