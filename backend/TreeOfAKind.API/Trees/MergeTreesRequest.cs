using System;
using System.ComponentModel.DataAnnotations;

namespace TreeOfAKind.API.Trees
{
    public class MergeTreesRequest
    {
        [Required] public Guid FirstTreeId { get; set; }
        [Required] public Guid SecondTreeId { get; set; }
    }
}