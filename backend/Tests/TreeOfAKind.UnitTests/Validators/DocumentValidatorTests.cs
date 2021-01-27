using System.IO;
using TreeOfAKind.Application.Command;
using Xunit;
using AutoFixture;
using FluentValidation;

namespace TreeOfAKind.UnitTests.Validators
{
    public class DocumentValidatorTests
    {
        DocumentValidator _validator = new DocumentValidator(new []{"acc", "bcc"});
        Fixture _fixture = new Fixture();

        [Theory]
        [InlineData("acc")]
        [InlineData("bcc")]
        private void Validate_ValidData_ValidationPasses(string contentType)
        {
            var document = new Document(Stream.Null, contentType, "name");

            Assert.True(_validator.Validate(document).IsValid);
        }

        [Theory]
        [InlineData("a")]
        [InlineData(" acc")]
        [InlineData(null)]
        private void Validate_InvalidContentType_ValidationFails(string contentType)
        {
            var document = new Document(Stream.Null, contentType, "name");

            Assert.False(_validator.Validate(document).IsValid);
        }

        [Fact]
        private void Validate_TooLongName_ValidationPasses()
        {
            var name = string.Join("", _fixture.CreateMany<char>(256));
            var document = new Document(Stream.Null, "acc", name);

            Assert.False(_validator.Validate(document).IsValid);
        }

        [Fact]
        private void Validate_NoContentProvided_ValidationPasses()
        {
            var document = new Document(null, "acc", "name");

            Assert.False(_validator.Validate(document).IsValid);
        }

        [Fact]
        private void Validate_NoNameProvided_ValidationPasses()
        {
            var document = new Document(Stream.Null, "", null);

            Assert.False(_validator.Validate(document).IsValid);
        }
    }
}
