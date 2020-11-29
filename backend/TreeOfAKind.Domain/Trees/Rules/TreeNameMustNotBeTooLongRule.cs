using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.Trees.Rules
{
    public class TreeNameMustNotBeTooLongRule : IBusinessRule
    {
        private readonly string _treeName;
        private const int NoLongerThan = 255;

        public TreeNameMustNotBeTooLongRule(string treeName)
        {
            _treeName = treeName;
        }

        public bool IsBroken()
            => _treeName?.Length > NoLongerThan;

        public string Message => $"Tree name must be shorter than {NoLongerThan}";
    }
}