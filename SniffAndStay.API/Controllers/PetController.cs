using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SniffAndStay.Application.Authentication.Commands;
using SniffAndStay.Application.Pets.Commands;
using SniffAndStay.Application.Pets.DTOs;
using SniffAndStay.Application.Pets.Queries;

namespace SniffAndStay.API.Controllers
{
    [ApiController]
    [Route("api/pet")]
    public class PetController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-user-pets-details/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserPetsDetails(Guid userId, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new GetUserPetsDetailsQuery(userId), cancellationToken);
            return Ok(response);
        }

        [HttpGet("get-user-pet-picture/{userId}/{petId}")]
        [Authorize]
        public async Task<IActionResult> GetUserPetPicture(Guid userId, Guid petId, CancellationToken cancellationToken)
        {
            var stream = await _mediator.Send(new GetUserPetPictureQuery(userId, petId), cancellationToken);

            if (stream is null)
                return NotFound();

            return File(stream, "image/jpeg");
        }

        [HttpPost("add-pet")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddPet(IFormFile? file, [FromQuery]PetRequest request, CancellationToken cancellationToken)
        {
            Stream? fileStream = file?.OpenReadStream();

            var response = await _mediator.Send(new AddPetCommand(request, fileStream, file?.FileName), cancellationToken);
            return Ok(response);
        }

        //[HttpPatch("update-pet/{petName}")]
        //[Authorize]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> UpdatePet(IFormFile? file, [FromQuery] UserProfileRequest request, CancellationToken cancellationToken)
        //{
        //    Stream? fileStream = file?.OpenReadStream();

        //    var response = await _mediator.Send(new UpdateUserProfileCommand(request, fileStream, file?.FileName), cancellationToken);
        //    return Ok(response);
        //}

        //[HttpDelete("delete-pet-picture/{petName}")]
        //[Authorize]
        //public async Task<IActionResult> DeleteUserPetPicture(string petName, CancellationToken cancellationToken)
        //{
        //    var response = await _mediator.Send(new DeleteUserProfilePictureCommand(null), cancellationToken);
        //    return Ok(response);
        //}

        //[HttpDelete("delete-pet/{petName}")]
        //public async Task DeletePet(string petName, CancellationToken cancellationToken)
        //{
        //    await _mediator.Send(new DeleteUserCommand(null), cancellationToken);
        //}
    }
}
