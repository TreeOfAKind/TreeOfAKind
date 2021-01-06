using System.Collections.Generic;
using Gx.Types;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public interface IGedcomXToDomainRelationTypeConverter
    {
        RelationType ConvertRelationType(RelationshipType knownType, IReadOnlyCollection<Domain.Trees.People.Person> people, PersonId to);
    }
}