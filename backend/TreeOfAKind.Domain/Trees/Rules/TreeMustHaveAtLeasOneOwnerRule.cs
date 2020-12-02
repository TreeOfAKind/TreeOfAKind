using System.Collections.Generic;
using System.Linq;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Domain.Trees.Rules
{
    public class TreeMustHaveAtLeasOneOwnerRule : IBusinessRule
    {
        public List<TreeUserProfile> TreeOwners { get; }

        public TreeMustHaveAtLeasOneOwnerRule(List<TreeUserProfile> treeOwners)
        {
            TreeOwners = treeOwners;
        }

        public bool IsBroken()
            => !TreeOwners.Any();

        public string Message => "Tree must have at least one owner. Consider deleting tree.";
    }
}