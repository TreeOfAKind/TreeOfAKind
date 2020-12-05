using System.IO;

namespace TreeOfAKind.Application.Command
{
    public class Document
    {
        public Stream Content { get; }
        public string ContentType { get; }

        public Document(Stream content, string contentType)
        {
            this.Content = content;
            ContentType = contentType;
        }
    }
}
