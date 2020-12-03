using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Command.Trees.RemoveTreeOwner
{
    public class RemoveTreeOwnerCommand : TreeOperationCommand
    {
        public UserId UserToRemoveId { get; }

        public RemoveTreeOwnerCommand(string requesterUserAuthId, TreeId treeId, UserId userToRemoveId)
            : base(requesterUserAuthId, treeId)
        {
            UserToRemoveId = userToRemoveId;
        }
    }
}