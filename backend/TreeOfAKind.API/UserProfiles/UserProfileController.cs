using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TreeOfAKind.API.SeedWork;
using TreeOfAKind.Application.Command.UserProfiles;
using TreeOfAKind.Application.Command.UserProfiles.CreateOrUpdateUserProfile;
using TreeOfAKind.Application.Query.UserProfiles.GetMyUserProfile;
using TreeOfAKind.Domain.UserProfiles;

namespace TreeOfAKind.API.UserProfiles
{
    [Route("api/[controller]/[action]")]

    public class UserProfileController : Controller
    {
        private readonly IMediator _mediator;

        public UserProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates or updates (if it already exists) User Profile for authenticated user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /UserProfile/CreateUserProfile
        ///     {
        ///        "firstName": "Bartosz",
        ///        "lastName": "Chrostowski",
        ///        "birthDate": "1998-02-12T00:00:00.000Z"
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Uuid of created user profile</returns>
        /// <response code="201">Returns uuid of created user profile</response>
        /// <response code="400">Command is not valid</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="422">Business rule broken</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(IdDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateOrUpdateUserProfile([FromBody] CreateOrUpdateUserProfileRequest request)
        {
            var userAuthId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userId = await _mediator.Send(
                new CreateOrUpdateUserProfileCommand(
                    userAuthId,
                    request.FirstName,
                    request.LastName,
                    request.BirthDate));

            return Created(string.Empty, new IdDto {Id = userId.Value});
        }

        
        
        /// <summary>
        /// Gets a User Profile of an authenticated user (from token).
        /// </summary>
        /// <returns>User profile of an authenticated user. May return null</returns>
        /// <response code="200">Returns user profile</response>
        /// <response code="401">User is not authenticated</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyUserProfile()
        {
            var authId = HttpContext.GetFirebaseUserAuthId();

            var user = await _mediator.Send(
                new GetMyUserProfileQuery(authId));

            return Ok(user);
        }
    }
}