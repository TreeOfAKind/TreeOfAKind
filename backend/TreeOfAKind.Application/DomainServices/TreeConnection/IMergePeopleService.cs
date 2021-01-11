using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.DomainServices.TreeConnection
{
    public interface IMergePeopleService
    {
        Person Merge(Person first, Person second, Tree tree);
    }
}