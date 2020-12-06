using System;
using System.ComponentModel.DataAnnotations;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.API.People
{
    public class AddRelationRequest
    {
        [Required] public Guid TreeId { get; set; }
        [Required] public Guid From { get; set; }
        [Required] public Guid To { get; set; }
        public RelationType RelationType { get; set; }
    }
}
