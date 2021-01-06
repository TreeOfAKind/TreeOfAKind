using System.Collections.Generic;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public interface IGedcomXToDomainRelationConverter
    {
        void AddRelationsToTree(Gx.Gedcomx gx, IDictionary<string, PersonId> gxIdToPersonId, Tree tree);
    }
}
