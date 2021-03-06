﻿using System;
using System.Net.Mail;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TreeOfAKind.API.SeedWork;
using TreeOfAKind.Application.Command;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.AddOrChangeTreePhoto;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.AddTreeOwner;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTree;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.CreateTreeFromFile;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.MergeTrees;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.RemoveMyselfFromTreeOwners;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.RemoveTreeOwner;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.RemoveTreePhoto;
using TreeOfAKind.Application.Command.Trees.TreeAdministration.UpdateTreeName;
using TreeOfAKind.Application.Query.Trees;
using TreeOfAKind.Application.Query.Trees.GetMyTrees;
using TreeOfAKind.Application.Query.Trees.GetTree;
using TreeOfAKind.Application.Query.Trees.GetTreeFileExport;
using TreeOfAKind.Application.Query.Trees.GetTreeStatistics;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;
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
            var mail = HttpContext.GetUserEmail();

            var result = await _mediator.Send(new CreateTreeCommand(request.TreeName, authId, new MailAddress(mail)));

            return Created(string.Empty, new IdDto {Id = result.Value});
        }

        /// <summary>
        /// Updates name of tree
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "treeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "treeName": "hmm"
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <response code="200">Tree name updated</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateTreeName([FromBody] UpdateTreeName request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            await _mediator.Send(new UpdateTreeNameCommand(authId, new TreeId(request.TreeId), request.TreeName));

            return Ok();
        }

        /// <summary>
        /// Gets tree with all of its details.
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
        /// <returns>Tree with all of its properties</returns>
        /// <response code="200">Returns tree</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(TreeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetTree([FromBody] GetTreeRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            var result = await _mediator.Send(new GetTreeQuery(authId, new TreeId(request.TreeId)));

            return Ok(result);
        }

        /// <summary>
        /// Gets statistics of a tree.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "treeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///     }
        ///
        /// NumberOfPeopleOfEachGender is a map from Gender
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Tree statistics</returns>
        /// <response code="200">Returns tree statistics</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(TreeStatisticsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetTreeStatistics([FromBody] GetTreeRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            var result = await _mediator.Send(new GetTreeStatisticsQuery(authId, new TreeId(request.TreeId)));

            return Ok(result);
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
        /// If `removedUserId` is empty, the requester will be removed.
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

            if (request.RemovedUserId.HasValue)
            {
                await _mediator.Send(new RemoveTreeOwnerCommand(authId, new TreeId(request.TreeId),
                    new UserId(request.RemovedUserId.Value)));
            }
            else
            {
                await _mediator.Send(new RemoveMyselfFromTreeOwnersCommand(authId, new TreeId(request.TreeId)));
            }

            return Ok();
        }

        /// <summary>
        /// Adds or changes (if one already exists) photo to tree.
        /// </summary>
        /// <remarks>
        /// Accepted content types are:
        ///
        ///     image/jpeg
        ///     image/png
        ///
        /// Accepts request as form data!!!
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Uri of created file</returns>
        /// <response code="201">Photo added (or changed if one already existed) to tree</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(UriDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddOrChangeTreePhoto([FromForm] AddTreePhotoRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            var image = request.Image;
            var uri = await _mediator.Send(new AddOrChangeTreePhotoCommand(authId, new TreeId(request.TreeId),
                new Document(image.OpenReadStream(), image.ContentType, image.FileName)));

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

        /// <summary>
        /// Gets file export of a tree.
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
        /// <response code="200"></response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetTreeFileExport([FromBody] GetTreeFileExport request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            var stream = await _mediator.Send(new GetTreeFileExportQuery(authId, new TreeId(request.TreeId)));

            return File(stream, "text/xml", request.TreeId.ToString() + ".xml");
        }

        /// <summary>
        /// Creates new tree based on a Gedcom X file.
        /// </summary>
        /// <remarks>
        /// Accepted content types of file are:
        ///
        ///     text/xml
        ///
        /// Accepts request as form data!!!
        /// </remarks>
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
        public async Task<IActionResult> CreateTreeFromFile([FromForm] CreateTreeFromFileRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();
            var mail = HttpContext.GetUserEmail();

            var file = request.File;
            var document = new Document(file.OpenReadStream(), file.ContentType, file.Name);
            var result = await _mediator.Send(new CreateTreeFromFileCommand(authId, new MailAddress(mail), document, request.TreeName));
            return Created(String.Empty, new IdDto(){Id = result.Value});
        }

        /// <summary>
        /// Merges two existing trees and creates new result tree.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "firstTreeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "secondTreeId": "72bef03b-62c2-4829-9917-bed803397de5"
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Uuid of result tree</returns>
        /// <response code="201">Returns uuid of result tree</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(IdDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> MergeTrees([FromBody] MergeTreesRequest request)
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            var result = await _mediator.Send(new MergeTreesCommand(new TreeId(request.FirstTreeId),
                new TreeId(request.SecondTreeId), authId));

            return Ok(new IdDto(){Id = result.Value});
        }
    }
}
