using System.IO;

namespace TreeOfAKind.Application.Command
{
    public class Document
    {
        public Stream Content { get; }
        public string ContentType { get; }
        public string Name { get; }

        public Document(Stream content, string contentType, string name)
        {
            this.Content = content;
            ContentType = contentType;
            Name = name;
        }
    }
}
