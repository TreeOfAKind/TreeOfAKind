using System;
using System.Collections.Generic;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Command.Trees.AddPerson
{
    public enum RelationDirection
    {
        FromAddedPerson,
        ToAddedPerson,
    }

    public class AddPersonCommand : TreeOperationCommandBase<PersonId>
    {
        public class Relation
        {
            public PersonId SecondPersonId { get; }
            public RelationDirection RelationDirection { get; }
            public RelationType RelationType { get; }

            public Relation(PersonId secondPersonId, RelationDirection relationDirection, RelationType relationType)
            {
                SecondPersonId = secondPersonId;
                RelationDirection = relationDirection;
                RelationType = relationType;
            }
        }
        public string? Name { get; }
        public string? Surname { get; }
        public Gender Gender { get; }
        public DateTime? BirthDate { get; }
        public DateTime? DeathDate { get; }
        public string? Description { get; }
        public string? Biography { get; }
        public IEnumerable<Relation> Relations { get; }

        public AddPersonCommand(string requesterUserAuthId, TreeId treeId, string? name, string? surname, Gender gender,
            DateTime? birthDate, DateTime? deathDate, string? description, string? biography,
            IEnumerable<Relation> relations = null) : base(
            requesterUserAuthId, treeId)
        {
            Name = name;
            Surname = surname;
            Gender = gender;
            BirthDate = birthDate;
            DeathDate = deathDate;
            Description = description;
            Biography = biography;
            Relations = relations ?? new List<Relation>();
        }
    }
}
