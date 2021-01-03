using System.IO;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Query.Trees.GetTreeFileExport
{
    public class GetTreeFileExportQuery : TreeQueryBase<Stream>
    {
        public GetTreeFileExportQuery(string requesterUserAuthId, TreeId treeId) : base(requesterUserAuthId, treeId)
        {
        }
    }
}
