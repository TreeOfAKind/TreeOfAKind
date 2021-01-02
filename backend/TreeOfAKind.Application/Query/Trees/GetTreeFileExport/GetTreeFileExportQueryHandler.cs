using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Application.Configuration.Queries;
using TreeOfAKind.Application.DomainServices;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Query.Trees.GetTreeFileExport
{
    public class GetTreeFileExportQueryHandler : IQueryHandler<GetTreeFileExportQuery, Stream>
    {
        private readonly ITreeRepository _treeRepository;
        private readonly IFamilyTreeFileExporter _familyTreeFileExporter;

        public GetTreeFileExportQueryHandler(ITreeRepository treeRepository, IFamilyTreeFileExporter familyTreeFileExporter)
        {
            _treeRepository = treeRepository;
            _familyTreeFileExporter = familyTreeFileExporter;
        }

        public async Task<Stream> Handle(GetTreeFileExportQuery request, CancellationToken cancellationToken)
        {
            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);

            var fileExport = _familyTreeFileExporter.Export(tree);

            return fileExport;
        }
    }
}
