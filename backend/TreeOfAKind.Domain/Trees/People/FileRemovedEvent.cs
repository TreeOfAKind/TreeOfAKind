using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.Trees.People
{
    public class FileRemovedEvent : DomainEventBase
    {
        private FileId FileId { get; }
        public FileRemovedEvent(FileId fileId)
        {
            FileId = fileId;
        }
    }
}