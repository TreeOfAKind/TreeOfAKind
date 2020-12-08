using System.Reflection;
using AutoFixture;
using TreeOfAKind.Application.Command.Trees.AddPerson;

namespace TreeOfAKind.UnitTests.Validators
{
    public class ValidatorTestsBase<TValidator> where TValidator : new()
    {
        protected readonly TValidator Validator;
        protected readonly Fixture Fixture = new Fixture();

        public ValidatorTestsBase()
        {
            Validator = new TValidator();
        }

        protected void SetPropertyWithReflection<T, TValue>(T instance, string fieldName, TValue value)
        {
            var field = typeof(T).GetField($"<{fieldName}>k__BackingField",
                BindingFlags.Instance | BindingFlags.NonPublic);
            field?.SetValue(instance, value);
        }
    }
}
