using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public interface IGedcomXToDomainTreeService
    {
        Tree ConvertTree(UserId userId, Gx.Gedcomx gx, string treeName);
    }
}