using System;
using System.Collections.Generic;
using System.Linq;
using Gx.Source;
using Gx.Types;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.DomainServices.GedcomXImport
{
    public class GedcomXToDomainPersonConverter : IGedcomXToDomainPersonConverter
    {
        private readonly IGedcomXToDomainGenderConverter _toDomainGenderConverter;
        private readonly IGedcomXNameExtractor _nameExtractor;
        private readonly IGedcomXDateExtractor _dateExtractor;
        private readonly IGedcomXNoteExtractor _noteExtractor;

        public GedcomXToDomainPersonConverter(IGedcomXToDomainGenderConverter gedcomXToDomainGenderConverter,
            IGedcomXNameExtractor nameExtractor, IGedcomXDateExtractor dateExtractor,
            IGedcomXNoteExtractor noteExtractor)
        {
            _nameExtractor = nameExtractor;
            _dateExtractor = dateExtractor;
            _noteExtractor = noteExtractor;
            _toDomainGenderConverter = gedcomXToDomainGenderConverter;
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
                    surname, _toDomainGenderConverter.ConvertGender(gxPerson.Gender?.KnownType ?? GenderType.NULL),
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
}
