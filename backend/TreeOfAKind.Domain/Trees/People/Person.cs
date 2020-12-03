using TreeOfAKind.Domain.SeedWork;

namespace TreeOfAKind.Domain.Trees.People
{
    public class Person : Entity
    {
        public PersonId Id { get; private set; }
        public Tree Tree { get; private set; }

        private Person()
        {
            Id = default!;
            Tree = default!;
        }
    }
}