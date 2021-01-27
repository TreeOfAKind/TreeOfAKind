using AutoFixture;
using TreeOfAKind.Application.Command.Trees.People.AddRelation;
using TreeOfAKind.Domain.Trees.People;
using Xunit;

namespace TreeOfAKind.UnitTests.Validators
{
    public class AddRelationValidatorTests : ValidatorTestsBase<AddRelationValidator>
    {
        [Fact]
        public void Validate_AutoFixtureGeneratedData_ValidationPasses()
        {
            var command = Fixture.Create<AddRelationCommand>();

            Assert.True(Validator.Validate(command).IsValid);
        }

        [Fact]
        public void Validate_IdNotProvided_ValidationFails()
        {
            var command = Fixture.Create<AddRelationCommand>();

            SetPropertyWithReflection(command, nameof(command.To),  (PersonId)null);

            Assert.False(Validator.Validate(command).IsValid);
        }

    }
}
