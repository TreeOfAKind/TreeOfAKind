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

            var person = tree!.AddPerson(
                request.Name,
                request.Surname,
                request.Gender,
                request.BirthDate,
                request.DeathDate,
                request.Description,
                request.Biography);

            foreach (var relation in request.Relations)
            {
                var (from, to) = relation.RelationDirection == RelationDirection.FromAddedPerson
                    ? (person.Id, relation.SecondPersonId)
                    : (relation.SecondPersonId, person.Id);

                tree.AddRelation(from,to,relation.RelationType);
            }

            return person.Id;
        }
    }
}
