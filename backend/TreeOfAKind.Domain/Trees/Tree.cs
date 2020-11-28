using System;
using System.Collections.Generic;
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
        public IReadOnlyCollection<UserProfile> TreeOwners =>
            _treeOwners.AsReadOnly();
        public IReadOnlyCollection<Person> People =>
            _people.AsReadOnly();

        private readonly List<UserProfile> _treeOwners 
            = new List<UserProfile>();

        private readonly List<Person> _people
            = new List<Person>();

        
        private Tree()
        {
			Id = default!;
			Name = default!;
        }
        private Tree(string name, UserProfile creator)
        {
            this.Id = new TreeId(Guid.NewGuid());
            this.Name = name;
            _treeOwners.Add(creator);
        }

        public static Tree CreateNewTree(string name, UserProfile creator)
        {
            CheckRule(new TreeNameMustNotBeTooLongRule(name));

            return new Tree(name, creator);
        }

        public void AddTreeOwner(UserProfile userProfile)
        {
            _treeOwners.Add(userProfile); 
            AddDomainEvent(new TreeOwnerAddedEvent(this, userProfile));
        }

        public void RemoveTreeOwner(UserId id)
        {
            _treeOwners.RemoveAll(o => o.Id == id);
            CheckRule(new TreeMustHaveAtLeasOneOwnerRule(_treeOwners));
            AddDomainEvent(new TreeOwnerRemovedEvent(this, id));
        }
    }
}
