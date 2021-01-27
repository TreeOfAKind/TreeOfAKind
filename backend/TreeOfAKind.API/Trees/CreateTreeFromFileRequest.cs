using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TreeOfAKind.API.Trees
{
    public class CreateTreeFromFileRequest
    {
        [Required] public string TreeName { get; set; }
        [Required] public IFormFile File { get; set; }
    }
}