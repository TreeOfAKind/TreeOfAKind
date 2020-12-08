using System;
using System.Reflection;
using AutoFixture;
using FluentValidation.Results;
using TreeOfAKind.Application.Command.Trees.AddPerson;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
using Xunit;

namespace TreeOfAKind.UnitTests.Validators
{
    public class AddPersonCommandValidatorTests : ValidatorTestsBase<AddPersonCommandValidator>
    {
        [Fact]
        public void Validate_AutoFixtureGeneratedData_ValidationPasses()
        {
            var command = Fixture.Create<AddPersonCommand>();

            Assert.True(Validator.Validate(command).IsValid);
        }

        [Fact]
        public void Validate_TooLongName_ValidationFails()
        {
            var command = Fixture.Create<AddPersonCommand>();

            SetPropertyWithReflection(command, nameof(command.Name),
                string.Join(string.Empty, Fixture.CreateMany<char>(StringLengths.Long)));

            Assert.False(Validator.Validate(command).IsValid);
        }

        [Fact]
        public void Validate_TooLongLastName_ValidationFails()
        {
            var command = Fixture.Create<AddPersonCommand>();

            SetPropertyWithReflection(command, nameof(command.LastName),
                string.Join(string.Empty, Fixture.CreateMany<char>(StringLengths.Long)));

            Assert.False(Validator.Validate(command).IsValid);
        }
    }
}
