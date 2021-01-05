using TreeOfAKind.Application.Query.Trees.GetTree;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Query.Trees.GetPerson
{
    public class GetPersonQuery : TreeQueryBase<PersonDto>
    {
        public PersonId PersonId { get; }
        public GetPersonQuery(string requesterUserAuthId, TreeId treeId, PersonId personId) : base(requesterUserAuthId, treeId)
        {
            PersonId = personId;
        }
    }
}
