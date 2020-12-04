using System;
using System.ComponentModel.DataAnnotations;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.API.People
{
    public class AddPersonRequest
    {
        [Required] public Guid TreeId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Gender Gender { get; set; }
        [DataType(DataType.Date)] public DateTime? BirthDate { get; set; }
        [DataType(DataType.Date)] public DateTime? DeathDate { get; set; }
        public string Description { get; set; }
        public string Biography { get; set; }
    }
}
