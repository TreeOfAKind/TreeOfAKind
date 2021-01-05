using System.Collections.Generic;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public interface IGedcomXToDomainRelationConverter
    {
        void AddRelations(Gx.Gedcomx gx, Dictionary<string, PersonId> gxIdToPersonId, Tree tree);
    }
}