using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public interface IGedcomXToDomainTreeConverter
    {
        Tree ConvertTree(UserId userId, Gx.Gedcomx gx, string treeName);
    }
}