using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TreeOfAKind.API.SeedWork;
using TreeOfAKind.API.UserProfiles;
using TreeOfAKind.Application.Command.Trees.People.AddPerson;
using TreeOfAKind.Application.Command.Trees.People.AddRelation;
using TreeOfAKind.Application.Command.Trees.People.RemovePerson;
using TreeOfAKind.Application.Command.Trees.People.RemoveRelation;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.API.People
{
    [Route("api/[controller]/[action]")]
    public class PeopleController : Controller
    {
        private readonly IMediator _mediator;

        public PeopleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates and adds new person to tree.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///          "treeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///          "name": "Bartek",
        ///          "surname": "Chrostowski",
        ///          "gender": "Male",
        ///          "birthDate": "1998-12-04",
        ///          "description": "Some guy",
        ///          "biography": "Loves his job 😍💕 at 🍑♻",
        ///          "relations": [
        ///             {
        ///                 "secondPerson": "72bef03b-62c2-4829-9917-bed803397de5",
        ///                 "direction": "FromAddedPerson",
        ///                 "relationType": "Father"
        ///             }
        ///          ]
        ///     }
        ///
        /// Direction specifies direction of a relation eg.
        ///
        /// for `direction = FromAddedPerson` and `relationType = Father`
        /// newly added person will be a father of a person with id specified
        /// in `secondPerson` property.
        ///
        /// similarly
        ///
        /// for `direction = ToAddedPerson` and `relationType = Father`
        /// newly added person will be a child of a person with id specified
        /// in `secondPerson` property (`secondPerson` will be father of newly
        /// added person)
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Uuid of created person</returns>
        /// <response code="201">Returns uuid of created person</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(IdDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddPerson([FromBody] AddPersonRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            var relations = request.Relations?.Select(r =>
                new AddPersonCommand.Relation(new PersonId(r.SecondPerson), r.Direction,
                    r.RelationType));

            var result = await _mediator.Send(
                new AddPersonCommand(authId, new TreeId(request.TreeId), request.Name, request.Surname, request.Gender,
                    request.BirthDate, request.DeathDate, request.Description, request.Biography, relations));


            return Created(string.Empty, new IdDto {Id = result.Value});
        }

        /// <summary>
        /// Removes person and all their relations from tree.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///          "treeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///          "personId": "72bef03b-62c2-4829-9917-bed803397de5"
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <response code="200">Person removed</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RemovePerson([FromBody] RemovePersonRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            await _mediator.Send(
                new RemovePersonCommand(authId, new TreeId(request.TreeId), new PersonId(request.PersonId)));

            return Ok();
        }

        /// <summary>
        /// Adds relation to tree.
        /// </summary>
        /// <remarks>
        /// Order of PersonIds in request affects action result.
        ///
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "treeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "from": "72bef03b-62c2-4829-9917-bed803397de5",
        ///        "to": "04520a87-6634-496a-9579-8fad94fba756",
        ///        "relationType": "Father"
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <response code="200">Relation added</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddRelation([FromBody] AddRelationRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            await _mediator.Send(
                new AddRelationCommand(authId, new TreeId(request.TreeId), new PersonId(request.From),
                    new PersonId(request.To), request.RelationType));

            return Ok();
        }

        /// <summary>
        /// Removes all relations between two people.
        /// </summary>
        /// <remarks>
        /// Order of PersonIds in request doesn't affect action result.
        ///
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "treeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "firstPersonId": "72bef03b-62c2-4829-9917-bed803397de5",
        ///        "secondPersonId": "04520a87-6634-496a-9579-8fad94fba756"
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <response code="200">Relation removed</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RemoveRelation([FromBody] RemoveRelationRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            await _mediator.Send(
                new RemoveRelationCommand(authId, new TreeId(request.TreeId), new PersonId(request.FirstPersonId),
                    new PersonId(request.SecondPersonId)));

            return Ok();
        }
    }
}
