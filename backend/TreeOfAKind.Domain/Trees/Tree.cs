using System;
using System.Collections.Generic;
using System.Linq;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees.People;
using TreeOfAKind.Domain.Trees.Rules;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Domain.Trees
{
    public class Tree : Entity, IAggregateRoot
    {
        public TreeId Id { get; private set; }
        public string Name { get; private set; }
        public IReadOnlyCollection<TreeUserProfile> TreeOwners =>
            _treeOwners;
        public IReadOnlyCollection<Person> People =>
            _people;

        private readonly List<TreeUserProfile> _treeOwners
            = new List<TreeUserProfile>();

        private readonly List<Person> _people
            = new List<Person>();


        private Tree()
        {
			Id = default!;
			Name = default!;
        }
        private Tree(string name, UserId creator)
        {
            this.Id = new TreeId(Guid.NewGuid());
            this.Name = name;
            _treeOwners.Add(new TreeUserProfile(creator, Id));
        }

        public static Tree CreateNewTree(string name, UserId creator)
        {
            CheckRule(new TreeNameMustNotBeTooLongRule(name));

            return new Tree(name, creator);
        }

        public void AddTreeOwner(UserId userId)
        {
            var treeUserProfile = new TreeUserProfile(userId, Id);

            if (_treeOwners.Contains(treeUserProfile)) return;

            _treeOwners.Add(treeUserProfile);
            AddDomainEvent(new TreeOwnerAddedEvent(Id, userId));
        }

        public void RemoveTreeOwner(UserId userId)
        {
            _treeOwners.RemoveAll(o => o.UserId == userId);
            CheckRule(new TreeMustHaveAtLeasOneOwnerRule(_treeOwners));
            AddDomainEvent(new TreeOwnerRemovedEvent(Id, userId));
        }
    }
}
