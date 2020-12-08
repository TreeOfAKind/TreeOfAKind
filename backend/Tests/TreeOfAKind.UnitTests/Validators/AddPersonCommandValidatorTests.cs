using AutoFixture;
using TreeOfAKind.Application.Command.Trees.People.AddPerson;
using TreeOfAKind.Application.Configuration;
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
        public void Validate_TooLongSurname_ValidationFails()
        {
            var command = Fixture.Create<AddPersonCommand>();

            SetPropertyWithReflection(command, nameof(command.Surname),
                string.Join(string.Empty, Fixture.CreateMany<char>(StringLengths.Long)));

            Assert.False(Validator.Validate(command).IsValid);
        }
    }
}
