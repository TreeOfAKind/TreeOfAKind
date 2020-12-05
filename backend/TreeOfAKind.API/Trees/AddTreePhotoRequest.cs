using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TreeOfAKind.API.Trees
{
    public class AddTreePhotoRequest
    {
        [Required] public Guid TreeId { get; set; }
        [Required] public IFormFile Image { set; get; }
    }
}
