using Gx.Types;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public class GedcomXToDomainGenderConverter : IGedcomXToDomainGenderConverter
    {
        public Gender ConvertGender(GenderType gender)
            => gender switch
            {
                GenderType.NULL => Gender.Unknown,
                GenderType.Male => Gender.Male,
                GenderType.Female => Gender.Female,
                GenderType.Unknown => Gender.Unknown,
                GenderType.OTHER => Gender.Other,
                _ => Gender.Unknown
            };
    }
}
