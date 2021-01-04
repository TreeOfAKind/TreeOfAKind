using System;
using System.Collections.Generic;
using System.Linq;
using Gx.Common;
using Gx.Conclusion;
using Gx.Source;
using Gx.Types;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using TreeOfAKind.Domain.UserProfiles;
using Gender = TreeOfAKind.Domain.Trees.People.Gender;
using Person = Gx.Conclusion.Person;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTreeFromFile
{
    public interface IGedcomXGenderConverter
    {
        public Gender ConvertGender(GenderType gender);
    }

    public class GedcomXGenderConverter : IGedcomXGenderConverter
    {
        public Gender ConvertGender(GenderType gender)
            => gender switch
            {
                GenderType.NULL => Gender.Unknown,
                GenderType.Male => Gender.Male,
                GenderType.Female => Gender.Female,
                GenderType.Unknown => Gender.Unknown,
                GenderType.OTHER => Gender.Other,
                _ => Gender.Unknown
            };
    }

    public interface IGedcomXNameExtractor
    {
        public string ExtractName(Person gxPerson, NamePartType namePartType);
        public string ExtractFullName(Person gxPerson);
    }

    public class GedcomXNameExtractor : IGedcomXNameExtractor
    {
        public string ExtractName(Person gxPerson, NamePartType namePartType)
        {
            return gxPerson.Names?.FirstOrDefault()?.NameForm?.Parts
                ?.FirstOrDefault(p => p.KnownType == namePartType)?.Value ?? "";
        }

        public string ExtractFullName(Person gxPerson)
        {
            return gxPerson.Names?.FirstOrDefault()?.NameForm?.FullText;
        }
    }

    public interface IGedcomXDateExtractor
    {
        public DateTime? GetDate(Person gxPerson, FactType factType);
    }

    public class GedcomXDateExtractor : IGedcomXDateExtractor
    {
        public DateTime? GetDate(Person gxPerson, FactType factType)
        {
            var fact = gxPerson?.Facts?.FirstOrDefault(f => f.KnownType == factType);

            if (DateTime.TryParse(fact?.Value, out DateTime date)) return date;

            return null;
        }
    }

    public interface IGedcomXNoteExtractor
    {
        string GetNote(IEnumerable<Note> notes, string subject);
    }

    public class GedcomXNoteExtractor : IGedcomXNoteExtractor
    {
        public string GetNote(IEnumerable<Note> notes, string subject)
            => notes?.FirstOrDefault(n => n.Subject == subject)?.Text ?? "";
    }

    public interface IGedcomXToDomainPersonConverter
    {
        Dictionary<string, PersonId> AddPeopleToTree(Gx.Gedcomx gx, Tree tree);
    }

    public class GedcomXToDomainPersonConverter : IGedcomXToDomainPersonConverter
    {
        private readonly IGedcomXGenderConverter _genderConverter;
        private readonly IGedcomXNameExtractor _nameExtractor;
        private readonly IGedcomXDateExtractor _dateExtractor;
        private readonly IGedcomXNoteExtractor _noteExtractor;

        public GedcomXToDomainPersonConverter(IGedcomXGenderConverter gedcomXGenderConverter,
            IGedcomXNameExtractor nameExtractor, IGedcomXDateExtractor dateExtractor,
            IGedcomXNoteExtractor noteExtractor)
        {
            _nameExtractor = nameExtractor;
            _dateExtractor = dateExtractor;
            _noteExtractor = noteExtractor;
            _genderConverter = gedcomXGenderConverter;
        }

        public Dictionary<string, PersonId> AddPeopleToTree(Gx.Gedcomx gx, Tree tree)
        {
            var gxIdToPersonId = new Dictionary<string, PersonId>();
            foreach (var gxPerson in gx.Persons)
            {
                var name = _nameExtractor.ExtractName(gxPerson, NamePartType.Given);
                var surname = _nameExtractor.ExtractName(gxPerson, NamePartType.Surname);

                if (name is null && surname is null)
                {
                    surname = _nameExtractor.ExtractFullName(gxPerson);
                }

                var person = tree.AddPerson(
                    name,
                    surname, _genderConverter.ConvertGender(gxPerson.Gender?.KnownType ?? GenderType.NULL),
                    _dateExtractor.GetDate(gxPerson, FactType.Birth),
                    _dateExtractor.GetDate(gxPerson, FactType.Death),
                    _noteExtractor.GetNote(gxPerson.Notes, "Description"),
                    _noteExtractor.GetNote(gxPerson.Notes, "Biography")
                );

                gxIdToPersonId.Add(gxPerson.Id ?? Guid.NewGuid().ToString(), person.Id);

                foreach (var source in gxPerson.Sources ?? Enumerable.Empty<SourceReference>())
                {
                    var mainPhoto = source.Qualifiers?.Any(q => q.Name == "MainPhoto" && bool.Parse(q.Value)) ?? false;
                    var fileName = source.Qualifiers?.FirstOrDefault(q => q.Name == "Name")?.Value;
                    var mimeType = source.Qualifiers?.FirstOrDefault(q => q.Name == "MimeType")?.Value;
                    var fileUri = source.Qualifiers?.FirstOrDefault(q => q.Name == "MainPhoto")?.Value;

                    if (fileName is null || mimeType is null || fileUri is null) continue;

                    if (mainPhoto)
                    {
                        person.AddOrChangeMainPhoto(fileName, mimeType, new Uri(fileUri));
                    }
                    else
                    {
                        person.AddFile(fileName, mimeType, new Uri(fileUri));
                    }
                }
            }

            return gxIdToPersonId;
        }
    }

    public interface IGedcomXToDomainRelationConverter
    {
        void AddRelations(Gx.Gedcomx gx, Dictionary<string, PersonId> gxIdToPersonId, Tree tree);
    }

    public interface IGedcomXToDomainRelationTypeConverter
    {
        RelationType ConvertRelationType(RelationshipType knownType, IReadOnlyCollection<Domain.Trees.People.Person> people, PersonId to);
    }
    public class GedcomXToDomainRelationTypeConverter : IGedcomXToDomainRelationTypeConverter
    {
        public RelationType ConvertRelationType(RelationshipType knownType, IReadOnlyCollection<Domain.Trees.People.Person> people, PersonId to)
        {
            return knownType switch
            {
                RelationshipType.Couple => RelationType.Spouse,
                RelationshipType.ParentChild => ParentChildRelationConverter(people, to),
                _ => RelationType.Unknown
            };
        }

        private static RelationType ParentChildRelationConverter(IReadOnlyCollection<Domain.Trees.People.Person> people,
            PersonId to)
        {
            var toPersonGender = people.FirstOrDefault(p => p.Id == to)?.Gender;
            return toPersonGender ==
                   Gender.Female
                ? RelationType.Mother
                : RelationType.Father;
        }
    }

    public class GedcomXToDomainRelationConverter : IGedcomXToDomainRelationConverter
    {
        private readonly IGedcomXToDomainRelationTypeConverter _gedcomXToDomainRelationTypeConverter;

        public GedcomXToDomainRelationConverter(IGedcomXToDomainRelationTypeConverter gedcomXToDomainRelationTypeConverter)
        {
            _gedcomXToDomainRelationTypeConverter = gedcomXToDomainRelationTypeConverter;
        }

        public void AddRelations(Gx.Gedcomx gx, Dictionary<string, PersonId> gxIdToPersonId, Tree tree)
        {
            var people = tree.People;
            foreach (var relationship in gx.Relationships)
            {
                if (gxIdToPersonId.TryGetValue(relationship.Person2.ResourceId, out var from)
                    && gxIdToPersonId.TryGetValue(relationship.Person1.ResourceId, out var to))
                {
                    var rel = _gedcomXToDomainRelationTypeConverter.ConvertRelationType(relationship.KnownType, people, to);
                    tree.AddRelation(from, to, rel);
                }
            }
        }
    }

    public class CreateTreeFromGedcomXService
    {
        private readonly IGedcomXToDomainRelationConverter _gedcomXToDomainRelationConverter;
        private readonly IGedcomXToDomainPersonConverter _gedcomXToDomainPersonConverter;

        public CreateTreeFromGedcomXService(IGedcomXToDomainPersonConverter gedcomXToDomainPersonConverter,
            IGedcomXToDomainRelationConverter gedcomXToDomainRelationConverter)
        {
            _gedcomXToDomainPersonConverter = gedcomXToDomainPersonConverter;
            _gedcomXToDomainRelationConverter = gedcomXToDomainRelationConverter;
        }

        public Tree Create(UserId userId, Gx.Gedcomx gx, string treeName)
        {
            var tree = Tree.CreateNewTree(treeName, userId);

            var gxIdToPersonId = _gedcomXToDomainPersonConverter.AddPeopleToTree(gx, tree);

            _gedcomXToDomainRelationConverter.AddRelations(gx, gxIdToPersonId, tree);

            return tree;
        }
    }
}
