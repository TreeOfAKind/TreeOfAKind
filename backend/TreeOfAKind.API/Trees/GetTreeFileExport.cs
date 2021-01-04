using System;
using System.ComponentModel.DataAnnotations;

namespace TreeOfAKind.API.Trees
{
    public class GetTreeFileExport
    {
        [Required] public Guid TreeId { get; set; }
    }
}