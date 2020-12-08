using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [DataType(DataType.Date)] public DateTime? BirthDate { get; set; }
        [DataType(DataType.Date)] public DateTime? DeathDate { get; set; }
        public string Description { get; set; }
        public string Biography { get; set; }
        public Guid? Mother { get; set; }
        public Guid? Father { get; set; }
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

            Mother = GetRelations(person.Id, RelationType.Mother, treeRelations).Cast<Guid?>().FirstOrDefault();
            Father = GetRelations(person.Id, RelationType.Father, treeRelations).Cast<Guid?>().FirstOrDefault();
            Spouses = GetRelations(person.Id, RelationType.Spouse, treeRelations);
            UnknownRelations = GetRelations(person.Id, RelationType.Unknown, treeRelations);
        }
    }

    public class TreeDto
    {
        public Guid TreeId { get; set; }
        public string TreeName { get; set; }
        public Uri PhotoUri { get; set; }
        public List<PersonDto> People { get; set; } = new List<PersonDto>();

        public TreeDto(Tree tree)
        {
            TreeId = tree.Id.Value;
            TreeName = tree.Name;
            PhotoUri = tree.Photo;
            People = tree.People.Select(p => new PersonDto(p, tree.TreeRelations)).ToList();
        }
    }
}
