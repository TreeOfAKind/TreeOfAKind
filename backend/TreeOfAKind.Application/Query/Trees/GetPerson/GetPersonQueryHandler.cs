using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Application.Configuration.Queries;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Query.Trees.GetPerson
{
    public class GetPersonQueryHandler : IQueryHandler<GetPersonQuery, PersonDto>
    {
        private readonly ITreeRepository _treeRepository;

        public GetPersonQueryHandler(ITreeRepository treeRepository)
        {
            _treeRepository = treeRepository;
        }

        public async Task<PersonDto> Handle(GetPersonQuery request, CancellationToken cancellationToken)
        {
            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);
            var person = tree?.People.FirstOrDefault(p => p.Id == request.PersonId);
            var relations = tree?.TreeRelations;
            return person is null ? null : new PersonDto(person, relations);
        }
    }
}
