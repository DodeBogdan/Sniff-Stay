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

        [HttpGet("get-user-profile-details")]
        [Authorize]
        public async Task<IActionResult> GetUserProfileDetails(CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new GetUserProfileDetailsQuery(), cancellationToken);
            return Ok(response);
        }

        [HttpGet("get-user-profile-picture")]
        [Authorize]
        public async Task<IActionResult> GetUserProfilePicture(CancellationToken cancellationToken)
        {
            var stream = await _mediator.Send(new GetUserProfilePictureQuery(), cancellationToken);

            if (stream is null)
                return NotFound();

            return File(stream, "image/jpeg");
        }

        [HttpPost("add-or-update-user-profile")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddOrUpdateUserProfile(IFormFile? file, [FromQuery] UserProfileRequest request, CancellationToken cancellationToken)
        {
            Stream? fileStream = file?.OpenReadStream();

            var response = await _mediator.Send(new AddOrUpdateUserProfileCommand(request, fileStream, file?.FileName), cancellationToken);
            return Ok(response);
        }

        [HttpDelete("delete-users-profile-picture")]
        [Authorize]
        public async Task<IActionResult> DeleteUserProfilePicture(Guid? userId, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new DeleteUserProfilePictureCommand(userId), cancellationToken);
            return Ok(response);
        }

#if DEBUG

        [HttpDelete("delete-users")]
        [Authorize(Roles = "Admin")]
        public async Task DeleteUser(string? email, bool? shouldRemoveAllUsers, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteUserCommand(email, shouldRemoveAllUsers ?? false), cancellationToken);
        }

#endif
    }
}
