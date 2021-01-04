using System.IO;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.DomainServices
{
    public interface IFamilyTreeFileExporter
    {
        public Stream Export(Tree tree);
    }
}