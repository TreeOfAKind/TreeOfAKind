using System.Collections.Generic;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public interface IGedcomXToDomainPersonConverter
    {
        Dictionary<string, PersonId> AddPeopleToTree(Gx.Gedcomx gx, Tree tree);
    }
}