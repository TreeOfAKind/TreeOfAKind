using System.Collections.Generic;

namespace TreeOfAKind.Application.Query.Trees.GetMyTrees
{
    public class TreesListDto
    {
        public List<TreeItemDto> Trees { get; set; } = new List<TreeItemDto>();
    }
}