using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Threading.Tasks;
using Google.Apis.Upload;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TreeOfAKind.API.SeedWork;
using TreeOfAKind.API.UserProfiles;
using TreeOfAKind.Application.Command.Trees.AddTreeOwner;
using TreeOfAKind.Application.Command.Trees.CreateTree;
using TreeOfAKind.Application.Command.Trees.RemoveTreeOwner;
using TreeOfAKind.Application.Query.Trees.GetMyTrees;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.API.Trees
{
    public class CreateTreeRequest
    {
        [Required]
        public string TreeName { get; set; }
    }
    
    public class AddTreeOwnerRequest
    {
        [Required]
        public Guid TreeId { get; set; } 
        [Required]
        public string InvitedUserEmail { get; set; }
    }
    
    [Route("api/[controller]/[action]")]
    public class TreeController : Controller
    {
        private readonly IMediator _mediator;

        public TreeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets users trees.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///     }
        ///
        /// Gets trees of authenticated user.
        /// </remarks>
        /// <returns>List of users trees</returns>
        /// <response code="201">Returns uuid of created tree</response>
        /// <response code="401">User is not authenticated</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(TreesListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyTrees()
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            var result = await _mediator.Send(new GetMyTreesQuery(authId));

            return Ok(result);
        }

        /// <summary>
        /// Creates new tree.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "treeName": "My new tree :)"
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Uuid of created tree</returns>
        /// <response code="201">Returns uuid of created tree</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(IdDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateTree([FromBody] CreateTreeRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            var result = await _mediator.Send(new CreateTreeCommand(request.TreeName, authId));
            
            return Created(string.Empty,new IdDto{ Id = result.Value});
        }

        /// <summary>
        /// Adds person as a tree owner.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "treeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "invitedUserEmail": "example@example.com"
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <response code="200">Tree owner added</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddTreeOwner([FromBody] AddTreeOwnerRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            var result = await _mediator.Send(new AddTreeOwnerCommand(new TreeId(request.TreeId),
                new MailAddress(request.InvitedUserEmail), authId));

            return Ok();
        }

        /// <summary>
        /// Removes tree owner.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "treeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "removedUserId": "72bef03b-62c2-4829-9917-bed803397de5"
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <response code="200">Tree owner removed</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RemoveTreeOwner([FromBody] RemoveTreeOwnerRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            var result = await _mediator.Send(new RemoveTreeOwnerCommand(authId, new TreeId(request.TreeId),
                new UserId(request.RemovedUserId)));

            return Ok();
        }
    }

    public class RemoveTreeOwnerRequest
    {
        [Required]
        public Guid TreeId { get; set; } 
        [Required]
        public Guid RemovedUserId { get; set; }
    }
}