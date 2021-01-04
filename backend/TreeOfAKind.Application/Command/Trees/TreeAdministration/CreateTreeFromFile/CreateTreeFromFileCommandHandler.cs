using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTreeFromFile
{
    public class CreateTreeFromFileCommandHandler : ICommandHandler<CreateTreeFromFileCommand, TreeId>
    {
        private readonly ITreeRepository _treeRepository;

        public CreateTreeFromFileCommandHandler(ITreeRepository treeRepository)
        {
            _treeRepository = treeRepository;
        }

        public Task<TreeId> Handle(CreateTreeFromFileCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult<TreeId>(null);
        }
    }
}
