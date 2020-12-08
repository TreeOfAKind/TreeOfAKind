using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.Trees.People
{
    public class NameOrSurnameMustBeSpecifiedRule : IBusinessRule
    {
        public string? Name { get; }
        public string? Surname { get; }
        public NameOrSurnameMustBeSpecifiedRule(string? name, string? surname)
        {
            Name = name;
            Surname = surname;
        }

        public bool IsBroken()
            => string.IsNullOrWhiteSpace(Name) && string.IsNullOrEmpty(Surname);

        public string Message => "Name or surname of person must be specified.";
    }
}