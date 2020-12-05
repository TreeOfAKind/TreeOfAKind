using System;
using System.Linq;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using TreeOfAKind.Domain.UserProfiles;
using Xunit;

namespace TreeOfAKind.UnitTests.Trees
{
    public class PeopleDomain
    {
        private Tree Tree { get; }
        private UserId OwnerId { get; }

        private static string Name
            => "Name";

        private static string Surname
            => "Surname";

        private static Gender Gender
            => Gender.Female;

        private static DateTime BirthDate
            => new DateTime(1900, 1, 12);

        private static DateTime DeathDate
            => new DateTime(1990, 3, 12);

        private static string Description
            => "This is some description";

        private static string Biography
            => "This is long biography: Lorem ipsum dolor " +
               "sit amet, consectetur adipiscing elit. Maecenas dolor est, " +
               "laoreet a convallis et, lacinia nec odio. Proin vel " +
               "interdum libero, id molestie tellus. Vestibulum a " +
               "justo neque. Duis fermentum dolor dui, id pharetra " +
               "ipsum elementum sit amet. Sed faucibus purus eu iaculis " +
               "luctus. Vestibulum ante ipsum primis in faucibus orci luctus " +
               "et ultrices posuere cubilia curae; Nulla purus leo, aliquet " +
               "a quam ut, pretium fringilla sem. Nulla facilisis nisl magna, " +
               "sit amet pellentesque ex rhoncus ac. Phasellus sodales, metus " +
               "volutpat ultrices faucibus, nulla arcu faucibus tellus, " +
               "a vehicula metus augue non neque. Praesent dictum, libero et " +
               "tincidunt auctor, enim arcu ullamcorper velit, vel malesuada " +
               "risus lacus nec neque. Donec quis ipsum eros. 😂🤣😂";

        public PeopleDomain()
        {
            OwnerId = new UserId(Guid.NewGuid());
            Tree = Tree.CreateNewTree("Some tree", OwnerId);
        }

        [Fact]
        public void AddPersonToTree_ValidData_PersonIsAdded()
        {
            var person = Tree.AddPerson(
                Name,
                Surname,
                Gender,
                BirthDate,
                DeathDate,
                Description,
                Biography);

            Assert.Single(Tree.People);
            Assert.Equal(person, Tree.People.FirstOrDefault());
        }

        [Fact]
        public void AddPersonToTree_NoNameNorSurnameProvided_ThrowsException()
        {
            Assert.Throws<BusinessRuleValidationException>(() =>
                Tree.AddPerson(
                    "",
                    null,
                    Gender,
                    BirthDate,
                    DeathDate,
                    Description,
                    Biography));
        }

        [Fact]
        public void AddPersonToTree_BirthDateAfterDeathDate_ThrowsException()
        {
            Assert.Throws<BusinessRuleValidationException>(() =>
                Tree.AddPerson(
                    Name,
                    Surname,
                    Gender,
                    DeathDate.AddDays(1),
                    DeathDate,
                    Description,
                    Biography)
            );
        }
    }
}
