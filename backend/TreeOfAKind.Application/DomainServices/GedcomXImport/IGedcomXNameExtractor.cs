using Gx.Conclusion;
using Gx.Types;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public interface IGedcomXNameExtractor
    {
        public string ExtractName(Person gxPerson, NamePartType namePartType);
        public string ExtractFullName(Person gxPerson);
    }
}