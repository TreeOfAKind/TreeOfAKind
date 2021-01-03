using System.IO;

namespace TreeOfAKind.Application.DomainServices
{
    public interface IGedcomToXmlStreamConverter
    {
        public Stream ToXmlStream(Gx.Gedcomx gx);
    }
}