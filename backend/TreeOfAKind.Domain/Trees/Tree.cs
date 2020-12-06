using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees.Events;
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
        public TreeRelations TreeRelations { get; private set; }


        private readonly List<TreeUserProfile> _treeOwners
            = new List<TreeUserProfile>();

        private readonly List<Person> _people
            = new List<Person>();


        private Tree()
        {
			Id = default!;
			Name = default!;
            TreeRelations = default!;
        }

        private Tree(string name, UserId creator)
        {
            Id = new TreeId(Guid.NewGuid());
            Name = name;
            TreeRelations = new TreeRelations();

            _treeOwners.Add(new TreeUserProfile(creator, Id));

            AddDomainEvent(new TreeCreatedEvent(Id));
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

        public Person AddPerson(
            string? name,
            string? surname,
            Gender gender,
            DateTime? birthDate,
            DateTime? deathDate,
            string? description,
            string? biography)
        {
            var person = Person.CreateNewPerson(
                this,
                name,
                surname,
                gender,
                birthDate,
                deathDate,
                description,
                biography);

            _people.Add(person);

            return person;
        }

        public void AddRelation(PersonId from, PersonId to, RelationType relationType)
        {
            CheckRule(new TreeMustContainPersonRule(People, from));
            CheckRule(new TreeMustContainPersonRule(People, to));

            TreeRelations.AddRelation(from,to,relationType);
        }

        public void RemoveRelation(PersonId first, PersonId second)
        {
            TreeRelations.RemoveRelation(first, second);
        }

        public void RemovePerson(PersonId personId)
        {
            TreeRelations.RemoveAllPersonRelations(personId);
            _people.RemoveAll(p => p.Id == personId);
        }

    }
}
