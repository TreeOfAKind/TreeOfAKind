using System;
using System.ComponentModel.DataAnnotations;

namespace TreeOfAKind.API.People
{
    public class RemoveRelationRequest
    {
        [Required] public Guid TreeId { get; set; }
        [Required] public Guid FirstPersonId { get; set; }
        [Required] public Guid SecondPersonId { get; set; }
    }
}