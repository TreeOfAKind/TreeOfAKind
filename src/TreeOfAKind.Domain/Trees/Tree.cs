using System;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees.Rules;

namespace TreeOfAKind.Domain.Trees
{
    public class Tree : Entity, IAggregateRoot
    {
        public TreeId Id { get; private set; }
        private string _name;

        private Tree()
        {
        }
        private Tree(string name)
        {
            this.Id = new TreeId(Guid.NewGuid());
            this._name = name;
        }

        public static Tree CreateNewTree(string name)
        {
            CheckRule(new TreeNameMustNotBeTooLong(name));

            return new Tree(name);
        }
    }
}