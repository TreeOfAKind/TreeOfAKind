

using AutoFixture;
using TreeOfAKind.Application.Command.Trees.People.RemoveRelation;
using TreeOfAKind.Domain.Trees.People;
using Xunit;

namespace TreeOfAKind.UnitTests.Validators
{
    public class RemoveRelationValidatorTests : ValidatorTestsBase<RemoveRelationValidator>
    {
        [Fact]
        public void Validate_AutoFixtureGeneratedData_ValidationPasses()
        {
            var command = Fixture.Create<RemoveRelationCommand>();

            Assert.True(Validator.Validate(command).IsValid);
        }

        [Fact]
        public void Validate_TooLongName_ValidationFails()
        {
            var command = Fixture.Create<RemoveRelationCommand>();

            SetPropertyWithReflection(command, nameof(command.First),  (PersonId)null);

            Assert.False(Validator.Validate(command).IsValid);
        }

        [Fact]
        public void Validate_TooLongSurname_ValidationFails()
        {
            var command = Fixture.Create<RemoveRelationCommand>();

            SetPropertyWithReflection(command, nameof(command.Second),(PersonId)null);

            Assert.False(Validator.Validate(command).IsValid);
        }
    }
}
