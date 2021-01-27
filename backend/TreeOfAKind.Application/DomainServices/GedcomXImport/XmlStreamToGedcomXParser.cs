using System.IO;
using System.Xml.Serialization;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public class XmlStreamToGedcomXParser : IXmlStreamToGedcomXParser
    {
        public Gx.Gedcomx Parse(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(Gx.Gedcomx));

            return (Gx.Gedcomx)serializer.Deserialize(stream);
        }
    }
}
