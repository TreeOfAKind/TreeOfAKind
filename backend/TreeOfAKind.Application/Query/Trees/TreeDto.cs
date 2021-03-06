﻿using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Application.Query.UserProfiles.GetMyUserProfile;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.Application.Query.Trees
{
    public class FileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public Uri Uri { get; set; }

        public FileDto(File file)
        {
            Id = file.Id.Value;
            Name = file.Name;
            ContentType = file.MimeType;
            Uri = file.FileUri;
        }
    }
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
        public Guid? Spouse { get; set; }
        public List<Guid> Children { get; set; }
        public List<Guid> UnknownRelations { get; set; }
        public FileDto MainPhoto { get; set; }
        public List<FileDto> Files { get; set; }

        private static List<Guid> GetRelations(PersonId id, RelationType relationType, TreeRelations treeRelations)
        {
            return treeRelations.Relations.Where(r => r.From == id && r.RelationType == relationType)
                .Select(t => t.To.Value).ToList();
        }

        private static List<Guid> GetChildren(PersonId id, TreeRelations treeRelations)
        {
            return treeRelations.Relations.Where(r =>
                    r.To == id && (r.RelationType == RelationType.Father || r.RelationType == RelationType.Mother))
                .Select(t => t.From.Value).ToList();
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
            Spouse = GetRelations(person.Id, RelationType.Spouse, treeRelations).Cast<Guid?>().FirstOrDefault();
            Children = GetChildren(person.Id, treeRelations);

            UnknownRelations = GetRelations(person.Id, RelationType.Unknown, treeRelations);

            MainPhoto = person.MainPhoto is null ? null : new FileDto(person.MainPhoto);
            Files = person.Files.Select(f => new FileDto(f)).ToList();
        }
    }

    public class TreeDto
    {
        public Guid TreeId { get; set; }
        public string TreeName { get; set; }
        public Uri PhotoUri { get; set; }
        public List<PersonDto> People { get; set; }

        public List<UserProfileDto> Owners { get; set; }

        public TreeDto(Tree tree, IEnumerable<UserProfile> userProfiles)
        {
            TreeId = tree.Id.Value;
            TreeName = tree.Name;
            PhotoUri = tree.Photo;
            People = tree.People.Select(p => new PersonDto(p, tree.TreeRelations)).ToList();
            Owners = userProfiles.Select(up => new UserProfileDto(up)).ToList();
        }
    }
}
