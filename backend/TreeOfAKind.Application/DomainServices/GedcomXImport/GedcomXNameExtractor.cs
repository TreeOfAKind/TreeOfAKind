using System.Linq;
using Gx.Conclusion;
using Gx.Types;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public class GedcomXNameExtractor : IGedcomXNameExtractor
    {
        public string ExtractName(Person gxPerson, NamePartType namePartType)
        {
            return gxPerson.Names?.FirstOrDefault()?.NameForm?.Parts
                ?.FirstOrDefault(p => p.KnownType == namePartType)?.Value ?? "";
        }

        public string ExtractFullName(Person gxPerson)
        {
            return gxPerson.Names?.FirstOrDefault()?.NameForm?.FullText;
        }
    }
}