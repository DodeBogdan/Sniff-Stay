using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SniffAndStay.Application.Authentication.Commands;
using SniffAndStay.Application.Authentication.Commands.BackdoorUser;
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

#if DEBUG

        /// <summary>
        /// This endpoint is only available in DEBUG mode and is used to retrieve a default user for testing purposes.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("get-default-user")]
        [AllowAnonymous]
        public async Task<AuthenticationResponse> GetDefaultUser(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetDefaultUserQuery(), cancellationToken);
        }

#endif

        /// <summary>
        /// This endpoint is used to authenticate a user and retrieve an authentication response containing the active token and refresh token.
        /// </summary>
        /// <param name="request">The login request containing the user's email and password.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<AuthenticationResponse> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new LoginCommand(request.Email, request.Password), cancellationToken);
        }

        /// <summary>
        /// This endpoint is used to register a new user.
        /// </summary>
        /// <param name="request">The registration request containing the user's email and password.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<AuthenticationResponse> Register(RegisterRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new RegisterCommand(request.Email, request.Password), cancellationToken);
        }

        /// <summary>
        /// This endpoint is used to logout a user.
        /// </summary>
        /// <param name="request">The token request containing the user's ID and refresh token.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("logout")]
        [Authorize]
        public async Task Logout(TokenRequest request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new LogoutCommand(request.UserId, request.RefreshToken), cancellationToken);
        }

        /// <summary>
        /// This endpoint is used to refresh an authentication token.
        /// </summary>
        /// <param name="request">The token request containing the user's ID and refresh token.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<AuthenticationResponse> RefreshToken(TokenRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new RefreshTokenCommand(request.UserId, request.RefreshToken), cancellationToken);
        }
    }
}
