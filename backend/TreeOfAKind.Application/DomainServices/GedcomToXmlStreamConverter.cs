using System.IO;
using System.Xml.Serialization;

namespace TreeOfAKind.Application.DomainServices
{
    public class GedcomToXmlStreamConverter : IGedcomToXmlStreamConverter
    {
        public Stream ToXmlStream(Gx.Gedcomx gx)
        {
            var serializer = new XmlSerializer(typeof(Gx.Gedcomx));
            var stream = new MemoryStream();

            serializer.Serialize(stream, gx);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}
