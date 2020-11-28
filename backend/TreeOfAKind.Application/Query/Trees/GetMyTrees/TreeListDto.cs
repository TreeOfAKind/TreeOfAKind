using System.Collections.Generic;

namespace TreeOfAKind.Application.Query.Trees.GetMyTrees
{
    public class TreeListDto
    {
        public List<TreeItemDto> Trees { get; set; } = new List<TreeItemDto>();
    }
}