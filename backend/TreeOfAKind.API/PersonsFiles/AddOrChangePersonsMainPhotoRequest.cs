using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TreeOfAKind.API.PersonsFiles
{
    public class AddOrChangePersonsMainPhotoRequest
    {
        [Required] public Guid TreeId { get; set; }
        [Required] public Guid PersonId { get; set; }
        [Required] public IFormFile File { get; set; }
    }
}