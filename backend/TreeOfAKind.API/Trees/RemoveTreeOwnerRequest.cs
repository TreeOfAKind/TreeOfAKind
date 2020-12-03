using System;
using System.ComponentModel.DataAnnotations;

namespace TreeOfAKind.API.Trees
{
    public class RemoveTreeOwnerRequest
    {
        [Required]
        public Guid TreeId { get; set; } 
        [Required]
        public Guid RemovedUserId { get; set; }
    }
}