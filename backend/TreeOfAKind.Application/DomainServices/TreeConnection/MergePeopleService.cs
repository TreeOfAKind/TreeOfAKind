using System;
using System.Collections.Generic;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.DomainServices.TreeConnection
{
    public class MergePeopleService : IMergePeopleService
    {
        private string NonEmpty(string first, string second)
            => string.IsNullOrEmpty(first) ? second : first;
        private Gender NonEmpty(Gender first, Gender second)
            => first == Gender.Unknown ? second : first;

        private DateTime? NonEmpty(DateTime? first, DateTime? second)
            => first ?? second;

        public Person Merge(Person first, Person second, Tree tree)
        {
            var firstName = NonEmpty(first.Name, second?.Name);
            var lastName = NonEmpty(first.LastName, second?.LastName);
            var description = NonEmpty(first.Description, second?.Description);
            var biography = NonEmpty(first.Biography, second?.Biography);
            var gender = NonEmpty(first.Gender, second?.Gender ?? Gender.Unknown);
            var birthDate = NonEmpty(first.BirthDate, second?.BirthDate);
            var deathDate = NonEmpty(first.DeathDate, second?.DeathDate);
            var person = tree.AddPerson(firstName, lastName, gender, birthDate, deathDate, description, biography);

            AddFiles(first.Files, tree, person);
            if (second is not null) AddFiles(second.Files, tree, person);

            var mainPhoto = first.MainPhoto ?? second?.MainPhoto;

            if (mainPhoto is not null)
            {
                tree.AddOrChangePersonsMainPhoto(person.Id, mainPhoto.Name, mainPhoto.MimeType, mainPhoto.FileUri);
            }

            return person;
        }

        private static void AddFiles(IEnumerable<File> files, Tree tree, Person person)
        {
            foreach (var file in files)
            {
                tree.AddPersonsFile(person.Id, file.Name, file.MimeType, file.FileUri);
            }
        }
    }
}
