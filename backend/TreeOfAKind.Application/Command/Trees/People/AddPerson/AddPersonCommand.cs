using System;
using System.Collections.Generic;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Command.Trees.People.AddPerson
{
    public class AddPersonCommand : TreeOperationCommandBase<PersonId>
    {
        public string? Name { get; }
        public string? LastName { get; }
        public Gender Gender { get; }
        public DateTime? BirthDate { get; }
        public DateTime? DeathDate { get; }
        public string? Description { get; }
        public string? Biography { get; }
        public IEnumerable<Relation> Relations { get; }

        public AddPersonCommand(string requesterUserAuthId, TreeId treeId, string? name, string? lastName, Gender gender,
            DateTime? birthDate, DateTime? deathDate, string? description, string? biography,
            IEnumerable<Relation> relations = null) : base(
            requesterUserAuthId, treeId)
        {
            Name = name;
            LastName = lastName;
            Gender = gender;
            BirthDate = birthDate;
            DeathDate = deathDate;
            Description = description;
            Biography = biography;
            Relations = relations ?? new List<Relation>();
        }
    }
}
