using System.IO;
using TreeOfAKind.Application.Command;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using Xunit;
using AutoFixture;
using TreeOfAKind.Application.Command.Trees.People.AddPersonsFile;

namespace TreeOfAKind.UnitTests.Validators
{
    public class AddPersonFileCommandValidatorTests : ValidatorTestsBase<AddPersonsFileCommandValidator>
    {
        [Theory]
        [InlineData("image/jpeg")]
        [InlineData("image/png")]
        [InlineData("application/pdf")]
        private void Validate_ValidData_ValidationPasses(string contentType)
        {
            var command = new AddPersonsFileCommand("AuthId", Fixture.Create<TreeId>(),
                new Document(Stream.Null, contentType, "name"), Fixture.Create<PersonId>());

            Assert.True(Validator.Validate(command).IsValid);
        }

        [Theory]
        [InlineData("image/gif")]
        [InlineData("asdf")]
        [InlineData("image/tiff")]
        [InlineData("")]
        [InlineData(null)]
        private void Validate_InvalidContentType_ValidationFails(string contentType)
        {
            var command = new AddPersonsFileCommand("AuthId", Fixture.Create<TreeId>(),
                new Document(Stream.Null, contentType, "name"), Fixture.Create<PersonId>());

            Assert.False(Validator.Validate(command).IsValid);
        }

        [Fact]
        private void Validate_NoContent_ValidationFails()
        {
            var command = new AddPersonsFileCommand("AuthId", Fixture.Create<TreeId>(),
                new Document(null, "image/jpeg", "name"), Fixture.Create<PersonId>());

            Assert.False(Validator.Validate(command).IsValid);
        }

        [Fact]
        private void Validate_NoName_ValidationFails()
        {
            var command = new AddPersonsFileCommand("AuthId", Fixture.Create<TreeId>(),
                new Document(Stream.Null, "image/jpeg", null), Fixture.Create<PersonId>());

            Assert.False(Validator.Validate(command).IsValid);
        }

        [Fact]
        private void Validate_NoDocument_ValidationFails()
        {
            var command = new AddPersonsFileCommand("AuthId", Fixture.Create<TreeId>(),
                null, Fixture.Create<PersonId>());

            Assert.False(Validator.Validate(command).IsValid);
        }
    }
}
