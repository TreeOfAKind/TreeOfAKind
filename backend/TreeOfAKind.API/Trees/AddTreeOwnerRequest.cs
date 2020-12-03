using System;
using System.ComponentModel.DataAnnotations;

namespace TreeOfAKind.API.Trees
{
    public class AddTreeOwnerRequest
    {
        [Required]
        public Guid TreeId { get; set; } 
        [Required]
        public string InvitedUserEmail { get; set; }
    }
}