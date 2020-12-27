using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TreeOfAKind.API.SeedWork;
using TreeOfAKind.Application.Command;
using TreeOfAKind.Application.Command.Trees.People.AddOrChangePersonsPhoto;
using TreeOfAKind.Application.Command.Trees.People.AddPersonFile;
using TreeOfAKind.Application.Command.Trees.People.RemovePersonsFile;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.API.PersonsFiles
{
    [Route("api/[controller]/[action]")]
    public class PersonsFilesController : Controller
    {
        private readonly IMediator _mediator;

        public PersonsFilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Adds file to person.
        /// </summary>
        /// <remarks>
        /// Accepted content types are:
        ///
        ///     image/jpeg
        ///     image/png
        ///     application/pdf
        ///
        /// Accepts body as from data!!!
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Uri and Id of created file</returns>
        /// <response code="201">File added to person</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(IdUriDto),StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddPersonsFile([FromForm] AddPersonsFileRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            var file = request.File;

            var result = await _mediator.Send(new AddPersonsFileCommand(authId, new TreeId(request.TreeId),
                new Document(file.OpenReadStream(), file.ContentType, file.Name), new PersonId(request.PersonId)));

            return Created(string.Empty, new IdUriDto{Id = result.Id.Value, Uri = result.FileUri});
        }

        /// <summary>
        /// Adds or changes (if one already exists) person's main photo.
        /// </summary>
        /// <remarks>
        /// Accepted content types are:
        ///
        ///     image/jpeg
        ///     image/png
        ///
        /// Accepts body as from data!!!
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Uri and Id of created main photo</returns>
        /// <response code="201">Main photo added to person</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(IdUriDto),StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddOrChangePersonsMainPhoto(
            [FromForm] AddOrChangePersonsMainPhotoRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            var file = request.File;

            var result = await _mediator.Send(new AddOrChangePersonsPhotoCommand(authId, new TreeId(request.TreeId),
                new Document(file.OpenReadStream(), file.ContentType, file.Name), new PersonId(request.PersonId)));

            return Created(string.Empty, new IdUriDto{Id = result.Id.Value, Uri = result.FileUri});
        }

        /// <summary>
        /// Remove file from person. Removed file may be person's main photo.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "treeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "personId": "72bef03b-62c2-4829-9917-bed803397de5",
        ///        "fileId": "680673e9-d1b9-46b4-831e-471f21cbda7a"
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <response code="200">File removed</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RemovePersonsFile([FromBody] RemovePersonsFileRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            await _mediator.Send(new RemovePersonsFileCommand(authId, new TreeId(request.TreeId),
                new FileId(request.FileId), new PersonId(request.PersonId)));

            return Ok();
        }
    }
}
