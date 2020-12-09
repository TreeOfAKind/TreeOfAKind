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
            Assert.Throws<BusinessRuleValidationException>(() => _tree.AddPersonsFile(new PersonId(Guid.Empty), "asdf", "asfd",
                _fixture.Create<Uri>()));
        }

        [Fact]
        private void AddPersonFile_PersonInTree_AddsFile()
        {
            var person = _tree.AddPerson("asdf", "asdf", Gender.Female, null, null, null, null);

            _tree.AddPersonsFile(person.Id, "asdf", "asfd",_fixture.Create<Uri>());

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
            var person = _tree.AddPerson("asdf", "asdf", Gender.Female, null, null, null, null);

            Assert.Null(person.MainPhoto);

            _tree.AddOrChangePersonsMainPhoto(person.Id, "asdf", "asfd",_fixture.Create<Uri>());

            Assert.NotNull(person.MainPhoto);
            Assert.Empty(person.Files);
        }

        [Fact]
        private void RemovePersonsFile_RemovingMainPhoto_FileRemoved()
        {
            var person = _tree.AddPerson("asdf", "asdf", Gender.Female, null, null, null, null);

            Assert.Null(person.MainPhoto);

            var mainPhotoId = _tree.AddOrChangePersonsMainPhoto(person.Id, "asdf", "asfd",_fixture.Create<Uri>());
            var fileId = _tree.AddPersonsFile(person.Id, "asdf", "asfd",_fixture.Create<Uri>());

            _tree.RemovePersonsFile(person.Id, mainPhotoId);
            Assert.Null(person.MainPhoto);
            Assert.Single(person.Files);
        }

        [Fact]
        private void RemovePersonsFile_RemovingFile_FileRemoved()
        {
            var person = _tree.AddPerson("asdf", "asdf", Gender.Female, null, null, null, null);

            Assert.Null(person.MainPhoto);

            var mainPhotoId = _tree.AddOrChangePersonsMainPhoto(person.Id, "asdf", "asfd",_fixture.Create<Uri>());
            var fileId = _tree.AddPersonsFile(person.Id, "asdf", "asfd",_fixture.Create<Uri>());

            _tree.RemovePersonsFile(person.Id, fileId);
            Assert.NotNull(person.MainPhoto);
            Assert.Empty(person.Files);
        }
    }
}
