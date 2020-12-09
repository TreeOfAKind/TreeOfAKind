using System;
using System.ComponentModel.DataAnnotations;

namespace TreeOfAKind.API.PersonsFiles
{
    public class RemovePersonsFileRequest
    {
        [Required] public Guid TreeId { get; set; }
        [Required] public Guid PersonId { get; set; }
        [Required] public Guid FileId { get; set; }
    }
}