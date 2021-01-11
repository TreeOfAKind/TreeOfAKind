using System.Collections.Generic;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public class GedcomXToDomainRelationConverter : IGedcomXToDomainRelationConverter
    {
        private readonly IGedcomXToDomainRelationTypeConverter _gedcomXToDomainRelationTypeConverter;

        public GedcomXToDomainRelationConverter(IGedcomXToDomainRelationTypeConverter gedcomXToDomainRelationTypeConverter)
        {
            _gedcomXToDomainRelationTypeConverter = gedcomXToDomainRelationTypeConverter;
        }

        public void AddRelationsToTree(Gx.Gedcomx gx, IDictionary<string, PersonId> gxIdToPersonId, Tree tree)
        {
            var people = tree.People;
            foreach (var relationship in gx.Relationships)
            {
                if (gxIdToPersonId.TryGetValue(relationship.Person2.Resource.Substring(1), out var from)
                    && gxIdToPersonId.TryGetValue(relationship.Person1.Resource.Substring(1), out var to))
                {
                    var rel = _gedcomXToDomainRelationTypeConverter.ConvertRelationType(relationship.Type, people, to);
                    tree.AddRelation(from, to, rel);
                }
            }
        }
    }
}
