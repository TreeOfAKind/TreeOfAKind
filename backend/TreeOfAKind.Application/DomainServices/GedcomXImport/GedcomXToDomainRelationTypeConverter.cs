using System.Collections.Generic;
using System.Linq;
using Gx.Types;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public class GedcomXToDomainRelationTypeConverter : IGedcomXToDomainRelationTypeConverter
    {
        public RelationType ConvertRelationType(RelationshipType knownType, IReadOnlyCollection<Domain.Trees.People.Person> people, PersonId to)
        {
            return knownType switch
            {
                RelationshipType.Couple => RelationType.Spouse,
                RelationshipType.ParentChild => ParentChildRelationConverter(people, to),
                _ => RelationType.Unknown
            };
        }

        private static RelationType ParentChildRelationConverter(IReadOnlyCollection<Domain.Trees.People.Person> people,
            PersonId to)
        {
            var toPersonGender = people.FirstOrDefault(p => p.Id == to)?.Gender;
            return toPersonGender ==
                   Gender.Female
                ? RelationType.Mother
                : RelationType.Father;
        }
    }
}