using System;
using System.ComponentModel.DataAnnotations;

namespace TreeOfAKind.API.Trees
{
    public class RemoveTreeOwnerRequest
    {
        [Required] public Guid TreeId { get; set; }
        public Guid? RemovedUserId { get; set; }
    }
}
