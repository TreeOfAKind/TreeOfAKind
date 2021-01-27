using System;
using System.ComponentModel.DataAnnotations;

namespace TreeOfAKind.API.Trees
{
    public class UpdateTreeName
    {
        [Required] public Guid TreeId { get; set; }
        public string TreeName { get; set; }
    }
}
