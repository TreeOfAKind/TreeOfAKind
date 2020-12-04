using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Command.Trees.AddPerson
{
    public class AddPersonCommandHandler : ICommandHandler<AddPersonCommand, PersonId>
    {
        private readonly ITreeRepository _treeRepository;

        public AddPersonCommandHandler(ITreeRepository treeRepository)
        {
            _treeRepository = treeRepository;
        }

        public async Task<PersonId> Handle(AddPersonCommand request, CancellationToken cancellationToken)
        {
            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);

            var personId = tree!.AddPerson(
                request.Name,
                request.Surname,
                request.Gender,
                request.BirthDate,
                request.DeathDate,
                request.Description,
                request.Biography);

            return personId.Id;
        }
    }
}
