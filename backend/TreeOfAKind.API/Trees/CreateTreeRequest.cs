using System.ComponentModel.DataAnnotations;

namespace TreeOfAKind.API.Trees
{
    public class CreateTreeRequest
    {
        [Required] public string TreeName { get; set; }
    }
}
