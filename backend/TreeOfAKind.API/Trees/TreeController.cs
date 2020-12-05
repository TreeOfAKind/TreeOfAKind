using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Google.Apis.Upload;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TreeOfAKind.API.SeedWork;
using TreeOfAKind.API.UserProfiles;
using TreeOfAKind.Application.Command;
using TreeOfAKind.Application.Command.Trees.AddOrChangeTreePhoto;
using TreeOfAKind.Application.Command.Trees.AddTreeOwner;
using TreeOfAKind.Application.Command.Trees.CreateTree;
using TreeOfAKind.Application.Command.Trees.RemoveTreeOwner;
using TreeOfAKind.Application.Command.Trees.RemoveTreePhoto;
using TreeOfAKind.Application.Query.Trees.GetMyTrees;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.API.Trees
{
    [Route("api/[controller]/[action]")]
    public class TreeController : Controller
    {
        private readonly IMediator _mediator;

        public TreeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets user's trees.
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
        /// <returns>List of user's trees</returns>
        /// <response code="200">List of user's trees</response>
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

            await _mediator.Send(new AddTreeOwnerCommand(authId, new TreeId(request.TreeId),
                request.InvitedUserEmail));

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

            await _mediator.Send(new RemoveTreeOwnerCommand(authId, new TreeId(request.TreeId),
                new UserId(request.RemovedUserId)));

            return Ok();
        }

        /// <summary>
        /// Adds or changes (if one already exists) photo to tree.
        /// </summary>
        /// <remarks>
        /// Accepted content types are:
        ///
        ///     image/jpg
        ///     image/jpeg
        ///     image/png
        ///
        /// Accepts body as from data!!!
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Uri of created file</returns>
        /// <response code="201">Photo added (or changed if one already existed) to tree</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(UriDto),StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddOrChangeTreePhoto([FromForm] AddTreePhotoRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            var image = request.Image;
            var uri = await _mediator.Send(new AddOrChangeTreePhotoCommand(authId, new TreeId(request.TreeId),
                new Document(request.Image.OpenReadStream(), request.Image.ContentType)));

            return Created(string.Empty, new UriDto {Uri = uri});
        }

        /// <summary>
        /// Removes photo from tree.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "treeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <response code="200">Photo removed</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RemoveTreePhoto([FromBody] RemoveTreePhotoRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            await _mediator.Send(new RemoveTreePhotoCommand(authId, new TreeId(request.TreeId)));

            return Ok();
        }
    }
}
