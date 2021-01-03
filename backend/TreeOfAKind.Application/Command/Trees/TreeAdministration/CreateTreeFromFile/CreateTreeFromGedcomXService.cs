using System;
using System.Collections.Generic;
using System.Linq;
using Gx.Common;
using Gx.Conclusion;
using Gx.Source;
using Gx.Types;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;
using Gender = TreeOfAKind.Domain.Trees.People.Gender;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTreeFromFile
{
    public interface IGedcomXGenderConverter
    {
        public Gender ConvertGender(Gx.Conclusion.Gender gender);
    }

    public class GedcomXGenderConverter : IGedcomXGenderConverter
    {
        public Gender ConvertGender(Gx.Conclusion.Gender gender)
            => gender?.KnownType switch
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
    }

    public class GedcomXNameExtractor : IGedcomXNameExtractor
    {
        public string ExtractName(Person gxPerson, NamePartType namePartType)
        {
            return gxPerson.Names?.FirstOrDefault()?.NameForm?.Parts
                ?.FirstOrDefault(p => p.KnownType == namePartType)?.Value ?? "";
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
        void AddPeopleToTree(Gx.Gedcomx gx, Tree tree);
    }

    public class GedcomXToDomainPersonConverter : IGedcomXToDomainPersonConverter
    {
        private readonly IGedcomXGenderConverter _genderConverter;
        private readonly IGedcomXNameExtractor _nameExtractor;
        private readonly IGedcomXDateExtractor _dateExtractor;
        private readonly IGedcomXNoteExtractor _noteExtractor;

        public GedcomXToDomainPersonConverter(IGedcomXGenderConverter gedcomXGenderConverter,
            IGedcomXNameExtractor nameExtractor, IGedcomXDateExtractor dateExtractor, IGedcomXNoteExtractor noteExtractor)
        {
            _nameExtractor = nameExtractor;
            _dateExtractor = dateExtractor;
            _noteExtractor = noteExtractor;
            _genderConverter = gedcomXGenderConverter;
        }

        public void AddPeopleToTree(Gx.Gedcomx gx, Tree tree)
        {
            foreach (var gxPerson in gx.Persons)
            {
                var name = _nameExtractor.ExtractName(gxPerson, NamePartType.Given);
                var surname = _nameExtractor.ExtractName(gxPerson, NamePartType.Surname);

                var person = tree.AddPerson(
                    name,
                    surname, _genderConverter.ConvertGender(gxPerson.Gender),
                    _dateExtractor.GetDate(gxPerson, FactType.Birth),
                    _dateExtractor.GetDate(gxPerson, FactType.Death),
                    _noteExtractor.GetNote(gxPerson.Notes, "Description"),
                    _noteExtractor.GetNote(gxPerson.Notes, "Biography")
                );
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
        }
    }

    public class CreateTreeFromGedcomXService
    {
        private readonly IGedcomXToDomainPersonConverter _gedcomXToDomainPersonConverter;

        public CreateTreeFromGedcomXService(IGedcomXToDomainPersonConverter gedcomXToDomainPersonConverter)
        {
            _gedcomXToDomainPersonConverter = gedcomXToDomainPersonConverter;
        }

        public Tree Create(UserId userId, Gx.Gedcomx gx, string treeName)
        {
            var tree = Tree.CreateNewTree(treeName, userId);

            _gedcomXToDomainPersonConverter.AddPeopleToTree(gx, tree);

            return null;
        }


    }
}
