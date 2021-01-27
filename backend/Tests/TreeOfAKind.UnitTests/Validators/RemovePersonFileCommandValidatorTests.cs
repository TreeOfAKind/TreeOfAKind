using System;
using Xunit;
using AutoFixture;
using FluentValidation;
using TreeOfAKind.Application.Command.Trees.People.RemovePersonsFile;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.UnitTests.Validators
{
    public class RemovePersonFileCommandValidatorTests : ValidatorTestsBase<RemovePersonsFileCommandValidator>
    {
        [Fact]
        private void Validate_ValidData_ValidationPasses()
        {
            var command = Fixture.Create<RemovePersonsFileCommand>();

            Assert.True(Validator.Validate(command).IsValid);
        }

        [Fact]
        private void Validate_NoPersonId_ValidationPasses()
        {
            var command = new RemovePersonsFileCommand("authId", new TreeId(Guid.Empty), new FileId(Guid.Empty), null);

            Assert.False(Validator.Validate(command).IsValid);
        }

        [Fact]
        private void Validate_NoFileId_ValidationPasses()
        {
            var command = new RemovePersonsFileCommand("authId", new TreeId(Guid.Empty), null, new PersonId(Guid.Empty));

            Assert.False(Validator.Validate(command).IsValid);
        }
    }
}
