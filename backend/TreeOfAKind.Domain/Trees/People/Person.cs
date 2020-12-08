using System;
using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.Trees.People
{
    public class Person : Entity
    {
        public PersonId Id { get; private set; }
        public Tree Tree { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public Gender Gender { get; private set; }
        public DateTime? BirthDate { get; private set; }
        public DateTime? DeathDate { get; private set; }
        public string Description { get; private set; }
        public string Biography { get; private set; }


        private Person()
        {
            Id = default!;
            Tree = default!;
            Name = default!;
            Surname = default!;
            Gender = default!;
            BirthDate = default!;
            DeathDate = default!;
            Description = default!;
            Biography = default!;
        }

        public Person(
            Tree tree,
            string? name,
            string? surname,
            Gender gender,
            DateTime? birthDate,
            DateTime? deathDate,
            string? description,
            string? biography)
        {
            Id = new PersonId(Guid.NewGuid());
            Tree = tree;
            Name = name ?? "";
            Surname = surname ?? "";
            Gender = gender;
            BirthDate = birthDate;
            DeathDate = deathDate;
            Description = description ?? "";
            Biography = biography ?? "";
        }

        public static Person CreateNewPerson(
            Tree tree,
            string? name,
            string? surname,
            Gender gender,
            DateTime? birthDate,
            DateTime? deathDate,
            string? description,
            string? biography)
        {
            CheckRule(new NameOrSurnameMustBeSpecifiedRule(name, surname));
            CheckRule(new BirthDateMustBeBeforeDeathDateRule(birthDate, deathDate));
            return new Person(tree, name, surname, gender, birthDate, deathDate, description, biography);
        }
    }
}