using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SniffAndStay.Application.Authentication.Commands;
using SniffAndStay.Application.Authentication.Commands.TODELETE;
using SniffAndStay.Application.Authentication.DTOs;
using SniffAndStay.Application.Authentication.Response;

namespace SniffAndStay.API.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// This will be deleted in production, it is only for testing purposes to get a default user and token.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("get-default-user")]
        [AllowAnonymous]
        public async Task<AuthenticationResponse> GetDefaultUser(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetDefaultUserQuery(), cancellationToken);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<AuthenticationResponse> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new LoginCommand(request.Email, request.Password), cancellationToken);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<AuthenticationResponse> Register(RegisterRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new RegisterCommand(request.Email, request.Password), cancellationToken);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task Logout(TokenRequest request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new LogoutCommand(request.UserId, request.RefreshToken), cancellationToken);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<AuthenticationResponse> RefreshToken(TokenRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new RefreshTokenCommand(request.UserId, request.RefreshToken), cancellationToken);
        }
    }
}
