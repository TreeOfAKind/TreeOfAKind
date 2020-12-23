using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Command.Trees.People.RemovePersonsFile
{
    public class RemovePersonsFileCommand : TreeOperationCommandBase
    {
        public FileId FileId { get; }
        public PersonId PersonId { get; }

        public RemovePersonsFileCommand(string requesterUserAuthId, TreeId treeId, FileId fileId, PersonId personId) : base(requesterUserAuthId, treeId)
        {
            FileId = fileId;
            PersonId = personId;
        }
    }
}
