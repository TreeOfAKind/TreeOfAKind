using System;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Command.Trees.AddPerson
{
    public class AddPersonCommand : TreeOperationCommandBase<PersonId>
    {
        public string? Name { get; }
        public string? Surname { get; }
        public Gender Gender { get; }
        public DateTime? BirthDate { get; }
        public DateTime? DeathDate { get; }
        public string? Description { get; }
        public string? Biography { get; }

        public AddPersonCommand(string requesterUserAuthId, TreeId treeId, string? name, string? surname, Gender gender,
            DateTime? birthDate, DateTime? deathDate, string? description, string? biography) : base(
            requesterUserAuthId, treeId)
        {
            Name = name;
            Surname = surname;
            Gender = gender;
            BirthDate = birthDate;
            DeathDate = deathDate;
            Description = description;
            Biography = biography;
        }
    }
}
