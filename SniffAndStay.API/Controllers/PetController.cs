using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Get the details of current the user's pets.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("get-user-pets-details")]
        [Authorize]
        public async Task<IActionResult> GetUserPetsDetails(CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new GetUserPetsDetailsQuery(), cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Get the picture of a specific pet by its ID from current user.
        /// </summary>
        /// <param name="petId">The ID of the pet whose picture to retrieve</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("get-user-pet-picture")]
        [Authorize]
        public async Task<IActionResult> GetUserPetPicture(Guid petId, CancellationToken cancellationToken)
        {
            var stream = await _mediator.Send(new GetUserPetPictureQuery(petId), cancellationToken);

            if (stream is null)
                return NotFound();

            return File(stream, "image/jpeg");
        }

        /// <summary>
        /// Add a new pet for the current user with an optional profile picture.
        /// </summary>
        /// <param name="file">The profile picture file for the pet</param>
        /// <param name="request">The request containing pet details</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        [HttpPost("add-pet")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddPet(IFormFile? file, [FromQuery]PetRequest request, CancellationToken cancellationToken)
        {
            Stream? fileStream = file?.OpenReadStream();

            var response = await _mediator.Send(new AddPetCommand(request, fileStream, file?.FileName, file?.ContentType), cancellationToken);
            return Ok(response);
        }


        [HttpDelete("remove-pet")]
        public async Task<IActionResult> RemovePet(Guid petId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new RemovePetCommand(petId), cancellationToken);
            return Ok("Pet removed successfully.");
        }
    }
}
