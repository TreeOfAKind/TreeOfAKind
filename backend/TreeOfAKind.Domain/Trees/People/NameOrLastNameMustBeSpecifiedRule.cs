using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.Trees.People
{
    public class NameOrLastNameMustBeSpecifiedRule : IBusinessRule
    {
        public string? Name { get; }
        public string? LastName { get; }
        public NameOrLastNameMustBeSpecifiedRule(string? name, string? lastName)
        {
            Name = name;
            LastName = lastName;
        }

        public bool IsBroken()
            => string.IsNullOrWhiteSpace(Name) && string.IsNullOrEmpty(LastName);

        public string Message => "Name or last name of person must be specified.";
    }
}
