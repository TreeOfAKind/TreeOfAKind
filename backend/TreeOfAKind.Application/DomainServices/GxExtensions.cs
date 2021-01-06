using System;
using System.Linq;
using Gx.Common;
using Gx.Conclusion;
using Gx.Source;
using Gx.Types;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.DomainServices
{
    public static class GxExtensions
    {
        public static Person AddNameLastname(this Person gedcomXPerson, Domain.Trees.People.Person person)
        {
            Name name;

            if (string.IsNullOrEmpty(person.Name))
            {
                var lastName = new NamePart(NamePartType.Surname, person.LastName);
                name = new Name(person.LastName, lastName);
            }
            else if (string.IsNullOrEmpty(person.LastName))
            {
                var firstName = new NamePart(NamePartType.Given, person.Name);
                name = new Name(person.Name, firstName);
            }
            else
            {
                var lastName = new NamePart(NamePartType.Surname, person.LastName);
                var firstName = new NamePart(NamePartType.Given, person.Name);
                name = new Name(person.Name + " " + person.LastName, firstName, lastName);
            }

            return gedcomXPerson.SetName(name);
        }

        private static Person AddDate(this Person gedcomXPerson, DateTime? date, FactType factType)
        {
            return date is null
                ? gedcomXPerson
                : gedcomXPerson.SetFact(new Fact(factType, date.ToString()));
        }

        public static Person AddBirthDate(this Person gedcomXPerson, Domain.Trees.People.Person person)
        {
            return gedcomXPerson.AddDate(person.BirthDate, FactType.Birth);
        }

        public static Person AddDeathDate(this Person gedcomXPerson, Domain.Trees.People.Person person)
        {
            return gedcomXPerson.AddDate(person.DeathDate, FactType.Death);
        }

        public static Person AddBiography(this Person gedcomXPerson, Domain.Trees.People.Person person)
        {
            var biography = new Note()
                .SetText(person.Biography)
                .SetSubject("Biography");
            return (Person) gedcomXPerson.SetNote(biography);
        }

        public static Person AddDescription(this Person gedcomXPerson, Domain.Trees.People.Person person)
        {
            var note = new Note()
                .SetText(person.Description)
                .SetSubject("Description");
            return (Person) gedcomXPerson.SetNote(note);
        }

        public static Person AddGender(this Person gedcomXPerson, Domain.Trees.People.Person person)
        {
            var genderType = person.Gender switch
            {
                Domain.Trees.People.Gender.Other => GenderType.OTHER,
                Domain.Trees.People.Gender.Male => GenderType.Male,
                Domain.Trees.People.Gender.Female => GenderType.Female,
                _ => GenderType.Unknown
            };

            return gedcomXPerson.SetGender(genderType);
        }

        public static Person AddFiles(this Person gedcomXPerson, Domain.Trees.People.Person person)
        {
            foreach (var file in person.Files)
            {
                var doc = new SourceReference();
                doc.AddQualifier(new Qualifier("Name", file.Name));
                doc.AddQualifier(new Qualifier("Uri", file.FileUri.ToString()));
                doc.AddQualifier(new Qualifier("MimeType", file.FileUri.ToString()));
                doc.AddQualifier(new Qualifier("MainPhoto", false.ToString()));
                gedcomXPerson.AddSource(doc);
            }

            if (person.MainPhoto is { })
            {
                var file = person.MainPhoto;
                var doc = new SourceReference();
                doc.AddQualifier(new Qualifier("Name", file.Name));
                doc.AddQualifier(new Qualifier("Uri", file.FileUri.ToString()));
                doc.AddQualifier(new Qualifier("MimeType", file.FileUri.ToString()));
                doc.AddQualifier(new Qualifier("MainPhoto", true.ToString()));
                gedcomXPerson.AddSource(doc);
            }

            return gedcomXPerson;
        }

        public static Gx.Gedcomx AddRelation(this Gx.Gedcomx gx, Domain.Trees.Relation relation, Person from, Person to)
        {
            var rel = new Relationship()
                .SetPerson1(to)
                .SetPerson2(from);

            rel = relation.RelationType switch
            {
                RelationType.Unknown => rel.SetType(RelationshipType.OTHER),
                RelationType.Father => rel.SetType(RelationshipType.ParentChild),
                RelationType.Mother => rel.SetType(RelationshipType.ParentChild),
                RelationType.Spouse => rel.SetType(RelationshipType.Couple),
                _ => rel.SetType(RelationshipType.NULL),
            };

            return gx.SetRelationship(rel);
        }
    }
}
