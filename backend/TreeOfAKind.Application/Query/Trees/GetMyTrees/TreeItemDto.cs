using System;

namespace TreeOfAKind.Application.Query.Trees.GetMyTrees
{
    public class TreeItemDto
    {
        public Guid Id { get; set; }
        public Uri PhotoUri { get; set; }
        public string TreeName { get; set; }
    }
}
