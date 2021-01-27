using TreeOfAKind.Application.Command.Trees.People.AddPerson;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Command.Trees.People
{
    public enum RelationDirection
    {
        FromAddedPerson,
        ToAddedPerson,
    }

    public class Relation
    {
        public PersonId SecondPersonId { get; }
        public RelationDirection RelationDirection { get; }
        public RelationType RelationType { get; }

        public Relation(PersonId secondPersonId, RelationDirection relationDirection, RelationType relationType)
        {
            SecondPersonId = secondPersonId;
            RelationDirection = relationDirection;
            RelationType = relationType;
        }
    }
}
