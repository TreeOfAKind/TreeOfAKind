using System.IO;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public interface IXmlStreamToGedcomXParser
    {
        Gx.Gedcomx Parse(Stream stream);
    }
}