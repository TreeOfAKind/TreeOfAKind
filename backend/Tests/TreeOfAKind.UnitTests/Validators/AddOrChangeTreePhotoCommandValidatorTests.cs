using System.IO;
using TreeOfAKind.Application.Command;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using Xunit;
using AutoFixture;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.AddOrChangeTreePhoto;

namespace TreeOfAKind.UnitTests.Validators
{
    public class AddOrChangeTreePhotoCommandValidatorTests : ValidatorTestsBase<AddOrChangeTreePhotoCommandValidator>
    {
        [Theory]
        [InlineData("image/jpeg")]
        [InlineData("image/png")]
        private void Validate_ValidData_ValidationPasses(string contentType)
        {
            var command = new AddOrChangeTreePhotoCommand("AuthId", Fixture.Create<TreeId>(),
                new Document(Stream.Null, contentType, "name"));

            Assert.True(Validator.Validate(command).IsValid);
        }

        [Theory]
        [InlineData("image/gif")]
        [InlineData("asdf")]
        [InlineData("image/tiff")]
        [InlineData("")]
        [InlineData("application/pdf")]
        [InlineData(null)]
        private void Validate_InvalidContentType_ValidationFails(string contentType)
        {
            var command = new AddOrChangeTreePhotoCommand("AuthId", Fixture.Create<TreeId>(),
                new Document(Stream.Null, contentType, "name"));

            Assert.False(Validator.Validate(command).IsValid);
        }

        [Fact]
        private void Validate_NoContent_ValidationFails()
        {
            var command = new AddOrChangeTreePhotoCommand("AuthId", Fixture.Create<TreeId>(),
                new Document(null, "image/jpeg", "name"));

            Assert.False(Validator.Validate(command).IsValid);
        }

        [Fact]
        private void Validate_NoName_ValidationFails()
        {
            var command = new AddOrChangeTreePhotoCommand("AuthId", Fixture.Create<TreeId>(),
                new Document(Stream.Null, "image/jpeg", null));

            Assert.False(Validator.Validate(command).IsValid);
        }

        [Fact]
        private void Validate_NoDocument_ValidationFails()
        {
            var command = new AddOrChangeTreePhotoCommand("AuthId", Fixture.Create<TreeId>(),
                null);

            Assert.False(Validator.Validate(command).IsValid);
        }
    }
}
