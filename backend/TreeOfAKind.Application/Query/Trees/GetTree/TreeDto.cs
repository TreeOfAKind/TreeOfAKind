using System;
using System.Collections.Generic;
using System.Linq;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Query.Trees.GetTree
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Gender Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public string Description { get; set; }
        public string Biography { get; set; }
        public List<Guid> Mothers { get; set; } = new List<Guid>();
        public List<Guid> Fathers { get; set; } = new List<Guid>();
        public List<Guid> Spouses { get; set; } = new List<Guid>();
        public List<Guid> UnknownRelations { get; set; } = new List<Guid>();

        private static List<Guid> GetRelations(PersonId id, RelationType relationType, TreeRelations treeRelations)
        {
            return treeRelations.Relations.Where(r => r.From == id && r.RelationType == relationType)
                .Select(t => t.To.Value).ToList();
        }

        public PersonDto(Person person, TreeRelations treeRelations)
        {
            Id = person.Id.Value;
            Name = person.Name;
            Surname = person.Surname;
            Gender = person.Gender;
            BirthDate = person.BirthDate;
            DeathDate = person.DeathDate;
            Description = person.Description;
            Biography = person.Biography;

            Mothers = GetRelations(person.Id, RelationType.Mother, treeRelations);
            Fathers = GetRelations(person.Id, RelationType.Father, treeRelations);
            Spouses = GetRelations(person.Id, RelationType.Spouse, treeRelations);
            UnknownRelations = GetRelations(person.Id, RelationType.Unknown, treeRelations);
        }
    }

    public class TreeDto
    {
        public Guid TreeId { get; set; }
        public string TreeName { get; set; }
        public List<PersonDto> People { get; set; } = new List<PersonDto>();

        public TreeDto(Tree tree)
        {
            TreeId = tree.Id.Value;
            TreeName = tree.Name;
            People = tree.People.Select(p => new PersonDto(p, tree.TreeRelations)).ToList();
        }
    }
}
