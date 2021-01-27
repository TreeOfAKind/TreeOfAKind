using System;
using System.ComponentModel.DataAnnotations;

namespace TreeOfAKind.API.Trees
{
    public class RemoveTreePhotoRequest
    {
        [Required] public Guid TreeId { get; set; }
    }
}
