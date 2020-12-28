using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.People.UpdatePerson
{
    public class UpdatePersonCommandHandler : ICommandHandler<UpdatePersonCommand, Unit>
    {
        private readonly ITreeRepository _treeRepository;

        public UpdatePersonCommandHandler(ITreeRepository treeRepository)
        {
            _treeRepository = treeRepository;
        }

        public async Task<Unit> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);

            tree!.UpdatePerson(
                request.PersonId,
                request.Name,
                request.LastName,
                request.Gender,
                request.BirthDate,
                request.DeathDate,
                request.Description,
                request.Biography);

            tree!.RemoveFromPersonRelations(request.PersonId);

            foreach (var relation in request.Relations)
            {
                var (from, to) = relation.RelationDirection == RelationDirection.FromAddedPerson
                    ? (request.PersonId, relation.SecondPersonId)
                    : (relation.SecondPersonId, request.PersonId);

                tree.AddRelation(from,to,relation.RelationType);
            }

            return Unit.Value;
        }
    }
}
