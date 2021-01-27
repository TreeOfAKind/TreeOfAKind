using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.People.RemovePerson
{
    public class RemovePersonCommandHandler : ICommandHandler<RemovePersonCommand, Unit>
    {
        private readonly ITreeRepository _treeRepository;

        public RemovePersonCommandHandler(ITreeRepository treeRepository)
        {
            _treeRepository = treeRepository;
        }

        public async Task<Unit> Handle(RemovePersonCommand request, CancellationToken cancellationToken)
        {
            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);

            tree!.RemovePerson(request.PersonId);

            return Unit.Value;
        }
    }
}
