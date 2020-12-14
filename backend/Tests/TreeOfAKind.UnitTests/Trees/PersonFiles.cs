using System;
using AutoFixture;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using TreeOfAKind.Domain.UserProfiles;
using Xunit;

namespace TreeOfAKind.UnitTests.Trees
{
    public class PersonFiles
    {
        private Tree _tree = Tree.CreateNewTree("treeName", new UserId(Guid.Empty));
        private Fixture _fixture = new Fixture();

        [Fact]
        private void AddPersonFile_PersonNotInTree_ThrowsException()
        {
            Assert.Throws<BusinessRuleValidationException>(() => _tree.AddPersonsFile(new PersonId(Guid.Empty), "ASDF.jpg", "image/jpg",
                _fixture.Create<Uri>()));
        }

        [Fact]
        private void AddPersonFile_PersonInTree_AddsFile()
        {
            var person = _tree.AddPerson("Name", "Surname", Gender.Female, null, null, null, null);

            _tree.AddPersonsFile(person.Id, "Name", "Surname",_fixture.Create<Uri>());

            Assert.Single(person.Files);
        }

        [Fact]
        private void AddOrChangePersonMainPhoto_PersonNotInTree_ThrowsException()
        {
            Assert.Throws<BusinessRuleValidationException>(() => _tree.AddOrChangePersonsMainPhoto(new PersonId(Guid.Empty), "asdf", "asfd",
                _fixture.Create<Uri>()));
        }

        [Fact]
        private void AddOrChangePersonMainPhoto_PersonInTree_AddsFile()
        {
            var person = _tree.AddPerson("Name", "Surname", Gender.Female, null, null, null, null);

            Assert.Null(person.MainPhoto);

            _tree.AddOrChangePersonsMainPhoto(person.Id, "Name", "Surname",_fixture.Create<Uri>());

            Assert.NotNull(person.MainPhoto);
            Assert.Empty(person.Files);
        }

        [Fact]
        private void RemovePersonsFile_RemovingMainPhoto_FileRemoved()
        {
            var person = _tree.AddPerson("Name", "Surname", Gender.Female, null, null, null, null);

            Assert.Null(person.MainPhoto);

            var mainPhoto = _tree.AddOrChangePersonsMainPhoto(person.Id, "ASDF.png", "image/png",_fixture.Create<Uri>());
            var file = _tree.AddPersonsFile(person.Id, "ASDF2.jpg", "image/jpg",_fixture.Create<Uri>());

            _tree.RemovePersonsFile(person.Id, mainPhoto.Id);
            Assert.Null(person.MainPhoto);
            Assert.Single(person.Files);
        }

        [Fact]
        private void RemovePersonsFile_RemovingFile_FileRemoved()
        {
            var person = _tree.AddPerson("Name", "Surname", Gender.Female, null, null, null, null);

            Assert.Null(person.MainPhoto);

            var mainPhoto = _tree.AddOrChangePersonsMainPhoto(person.Id, "asdf.png", "image/png",_fixture.Create<Uri>());
            var file = _tree.AddPersonsFile(person.Id, "asdf2.png", "image/png",_fixture.Create<Uri>());

            _tree.RemovePersonsFile(person.Id, file.Id);
            Assert.NotNull(person.MainPhoto);
            Assert.Empty(person.Files);
        }
    }
}
