using System.Collections.Generic;
using System.Linq;
using Gedcomx.Model.Util;
using Gx.Types;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public class GedcomXToDomainRelationTypeConverter : IGedcomXToDomainRelationTypeConverter
    {
        private const string Mother = "Mother";
        private const string Father = "Father";

        public RelationType ConvertRelationType(string type, IReadOnlyCollection<Domain.Trees.People.Person> people, PersonId to)
            => type switch
            {
                Mother => RelationType.Mother,
                Father => RelationType.Father,
                _ => XmlQNameEnumUtil.GetEnumValue<RelationshipType>(type) switch
                {
                    RelationshipType.Couple => RelationType.Spouse,
                    RelationshipType.ParentChild => ParentChildRelationConverter(people, to),
                    _ => RelationType.Unknown
                }
            };

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
