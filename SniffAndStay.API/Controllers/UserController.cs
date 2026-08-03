using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SniffAndStay.Application.Authentication.Commands;
using SniffAndStay.Application.Users.Commands;
using SniffAndStay.Application.Users.DTOs;
using SniffAndStay.Application.Users.Queries;

namespace SniffAndStay.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get the details of the currently authenticated user.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("get-user-profile-details")]
        [Authorize]
        public async Task<IActionResult> GetUserProfileDetails(CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new GetUserProfileDetailsQuery(), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Get the profile picture of the currently authenticated user.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("get-user-profile-picture")]
        [Authorize]
        public async Task<IActionResult> GetUserProfilePicture(CancellationToken cancellationToken)
        {
            var stream = await _mediator.Send(new GetUserProfilePictureQuery(), cancellationToken);

            if (stream is null)
                return NotFound();

            return File(stream, "image/jpeg");
        }

        /// <summary>
        /// Add or update the profile details of the currently authenticated user, including an optional profile picture.
        /// </summary>
        /// <param name="file">Picture file</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("add-or-update-user-profile")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddOrUpdateUserProfile(IFormFile? file, [FromQuery] UserProfileRequest request, CancellationToken cancellationToken)
        {
            Stream? fileStream = file?.OpenReadStream();

            var response = await _mediator.Send(new AddOrUpdateUserProfileCommand(request, fileStream, file?.FileName), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Delete the profile picture of the currently authenticated user.
        /// </summary>
        /// <param name="userId">The ID of the user whose profile picture to delete</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("delete-users-profile-picture")]
        [Authorize]
        public async Task<IActionResult> DeleteUserProfilePicture(Guid? userId, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new DeleteUserProfilePictureCommand(userId), cancellationToken);
            return Ok(response);
        }

#if DEBUG

        /// <summary>
        /// Delete a user by email or delete all users if shouldRemoveAllUsers is true.
        /// This endpoint is only available in DEBUG mode and requires Admin role authorization.
        /// </summary>
        /// <param name="email">The email of the user to delete</param>
        /// <param name="shouldRemoveAllUsers">Whether to delete all users</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("delete-users")]
        [Authorize(Roles = "Admin")]
        public async Task DeleteUser(string? email, bool? shouldRemoveAllUsers, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteUserCommand(email, shouldRemoveAllUsers ?? false), cancellationToken);
        }

#endif
    }
}
