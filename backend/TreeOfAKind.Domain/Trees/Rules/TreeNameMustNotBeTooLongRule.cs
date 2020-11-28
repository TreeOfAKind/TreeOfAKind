using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.Trees.Rules
{
    public class TreeNameMustNotBeTooLongRule : IBusinessRule
    {
        private const int NoLongerThan = 200;
        private readonly string _treeName;

        public TreeNameMustNotBeTooLongRule(string treeName)
        {
            _treeName = treeName;
        }

        public bool IsBroken()
        {
            throw new System.NotImplementedException();
        }

        public string Message => $"Tree name must be shorter than {NoLongerThan}";
    }
}