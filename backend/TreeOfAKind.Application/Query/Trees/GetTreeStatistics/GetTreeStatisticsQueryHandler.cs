using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gx.Types;
using TreeOfAKind.Application.Configuration.Queries;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Query.Trees.GetTreeStatistics
{
    public class GetTreeStatisticsQueryHandler : IQueryHandler<GetTreeStatisticsQuery, TreeStatisticsDto>
    {
        private readonly ITreeRepository _treeRepository;

        public GetTreeStatisticsQueryHandler(ITreeRepository treeRepository)
        {
            _treeRepository = treeRepository;
        }

        public async Task<TreeStatisticsDto> Handle(GetTreeStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            var tree = await _treeRepository.GetByIdAsync(request.TreeId,
                cancellationToken);

            var totalPeople = tree!.People.Count;

            var avgLifeSpan = tree.People
                .Where(p => p.BirthDate.HasValue && p.DeathDate.HasValue)
                .Select(p => p.DeathDate.Value - p.BirthDate.Value)
                .Select(ts => ts.TotalDays)
                .DefaultIfEmpty(0)
                .Average();

            var genderCount = tree.People
                .Select(p => p.Gender)
                .GroupBy(g => g)
                .ToDictionary(g => g.Key,
                    g => g.Count());

            var living = tree.People
                .Count(p => !p.DeathDate.HasValue);

            var dead = tree.People
                .Count(p => p.DeathDate.HasValue);

            var avgNumberOfChildren = tree.TreeRelations.Relations
                .Where(
                    r => r.RelationType == RelationType.Father || r.RelationType == RelationType.Mother)
                .GroupBy(r => r.To)
                .Select(group => group.Count())
                .DefaultIfEmpty(0)
                .Average();

            var noMarried = tree.People
                .Count(p => tree.TreeRelations.Relations.Any(
                    r => r.RelationType == RelationType.Spouse && r.From == p.Id));

            var noSingle = totalPeople - noMarried;

            return new TreeStatisticsDto(totalPeople,
                avgLifeSpan,
                genderCount,
                living,
                dead,
                avgNumberOfChildren,
                noMarried,
                noSingle);
        }
    }
}
