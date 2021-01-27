using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.Trees.People
{
    public class Person : Entity
    {
        public PersonId Id { get; private set; }
        public Tree Tree { get; private set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public string Description { get; set; }
        public string Biography { get; set; }
        public File? MainPhoto { get; private set; }
        public IReadOnlyCollection<File> Files =>
            _files;

        private readonly List<File>_files
            = new List<File>();


        private Person()
        {
            Id = default!;
            Tree = default!;
            Name = default!;
            LastName = default!;
            Gender = default!;
            BirthDate = default!;
            DeathDate = default!;
            Description = default!;
            Biography = default!;
        }

        public Person(
            Tree tree,
            string? name,
            string? lastName,
            Gender gender,
            DateTime? birthDate,
            DateTime? deathDate,
            string? description,
            string? biography)
        {
            Id = new PersonId(Guid.NewGuid());
            Tree = tree;
            Name = name ?? "";
            LastName = lastName ?? "";
            Gender = gender;
            BirthDate = birthDate;
            DeathDate = deathDate;
            Description = description ?? "";
            Biography = biography ?? "";
        }

        public static Person CreateNewPerson(
            Tree tree,
            string? name,
            string? lastName,
            Gender gender,
            DateTime? birthDate,
            DateTime? deathDate,
            string? description,
            string? biography)
        {
            CheckRule(new NameOrLastNameMustBeSpecifiedRule(name, lastName));
            CheckRule(new BirthDateMustBeBeforeDeathDateRule(birthDate, deathDate));
            return new Person(tree, name, lastName, gender, birthDate, deathDate, description, biography);
        }

        public File AddFile(string name, string mimeType, Uri fileUri)
        {
            var file = new File(name, mimeType, fileUri, this);
            _files.Add(file);
            return file;
        }

        public void RemoveFile(FileId fileId)
        {
            AddDomainEvent(new FileRemovedEvent(fileId));
            _files.RemoveAll(f => f.Id == fileId);
            if (MainPhoto?.Id == fileId) MainPhoto = null;
        }

        public File AddOrChangeMainPhoto(string name, string mimeType, Uri fileUri)
        {
            MainPhoto = new File(name, mimeType, fileUri, this);
            return MainPhoto;
        }
    }
}
