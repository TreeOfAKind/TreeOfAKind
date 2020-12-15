using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NodaTime;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Query.Trees.GetTree
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public LocalDate? BirthDate { get; set; }
        public LocalDate? DeathDate { get; set; }
        public string Description { get; set; }
        public string Biography { get; set; }
        public Guid? Mother { get; set; }
        public Guid? Father { get; set; }
        public List<Guid> Spouses { get; set; } = new List<Guid>();
        public List<Guid> Children { get; set; } = new List<Guid>();
        public List<Guid> UnknownRelations { get; set; } = new List<Guid>();
        private static List<Guid> GetRelations(PersonId id, RelationType relationType, TreeRelations treeRelations)
        {
            return treeRelations.Relations.Where(r => r.From == id && r.RelationType == relationType)
                .Select(t => t.To.Value).ToList();
        }

        private static List<Guid> GetChildren(PersonId id, TreeRelations treeRelations)
        {
            return treeRelations.Relations.Where(r =>
                    r.To == id && (r.RelationType == RelationType.Father || r.RelationType == RelationType.Mother))
                .Select(t => t.To.Value).ToList();
        }

        public PersonDto(Person person, TreeRelations treeRelations)
        {
            Id = person.Id.Value;
            Name = person.Name;
            LastName = person.LastName;
            Gender = person.Gender;
            BirthDate = person.BirthDate?.GetDate();
            DeathDate = person.DeathDate?.GetDate();
            Description = person.Description;
            Biography = person.Biography;

            Mother = GetRelations(person.Id, RelationType.Mother, treeRelations).Cast<Guid?>().FirstOrDefault();
            Father = GetRelations(person.Id, RelationType.Father, treeRelations).Cast<Guid?>().FirstOrDefault();
            Spouses = GetRelations(person.Id, RelationType.Spouse, treeRelations);
            Children = GetChildren(person.Id, treeRelations);

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
