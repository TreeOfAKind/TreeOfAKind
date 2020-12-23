using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreeOfAKind.Application.Command.Trees.People.AddPerson;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.API.People
{
    public class Relation
    {
        [Required] public Guid SecondPerson { get; set; }
        [Required] public RelationDirection Direction { get; set; }
        public RelationType RelationType { get; set; }
    }

    public class AddPersonRequest
    {
        [Required] public Guid TreeId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        [DataType(DataType.Date)] public DateTime? BirthDate { get; set; }
        [DataType(DataType.Date)] public DateTime? DeathDate { get; set; }
        public string Description { get; set; }
        public string Biography { get; set; }
        public Guid? Mother { get; set; }
        public Guid? Father { get; set; }
        public Guid? Spouse { get; set; }
    }
}
