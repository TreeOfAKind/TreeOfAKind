using System;
using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.Trees.People
{
    public class File : Entity
    {
        public FileId Id { get; private set; }
        public Person Owner { get; private set; }
        public Uri FileUri { get; private set; }
        public string Name { get; private set; }
        public string MimeType { get; private set; }

        private File()
        {
            Id = default!;
            Owner = default!;
            FileUri = default!;
            Name = default!;
            MimeType = default!;
        }

        public File(string name, string mimeType, Uri fileUri, Person owner)
        {
            Id = new FileId(Guid.NewGuid());
            Owner = owner;
            Name = name;
            MimeType = mimeType;
            FileUri = fileUri;
        }
    }
}