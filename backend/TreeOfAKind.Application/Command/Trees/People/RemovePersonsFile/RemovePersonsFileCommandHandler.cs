using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.People.RemovePersonsFile
{
    public class RemovePersonsFileCommandHandler : ICommandHandler<RemovePersonsFileCommand, Unit>
    {
        private readonly ITreeRepository _treeRepository;

        public RemovePersonsFileCommandHandler(ITreeRepository treeRepository)
        {
            _treeRepository = treeRepository;
        }

        public async Task<Unit> Handle(RemovePersonsFileCommand request, CancellationToken cancellationToken)
        {
            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);

            tree!.RemovePersonsFile(request.PersonId, request.FileId);

            return Unit.Value;
        }
    }
}
