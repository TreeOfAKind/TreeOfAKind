using Gx.Types;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public interface IGedcomXToDomainGenderConverter
    {
        public Gender ConvertGender(GenderType gender);
    }
}