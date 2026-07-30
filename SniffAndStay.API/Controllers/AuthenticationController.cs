using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SniffAndStay.Application.Authentication.Commands.DeleteUser;
using SniffAndStay.Application.Authentication.Commands.Login;
using SniffAndStay.Application.Authentication.Commands.Logout;
using SniffAndStay.Application.Authentication.Commands.RefreshToken;
using SniffAndStay.Application.Authentication.Commands.Register;
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
        [Authorize]
        public async Task<AuthenticationResponse> RefreshToken(TokenRequest request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new RefreshTokenCommand(request.UserId, request.RefreshToken), cancellationToken);
        }

        [HttpDelete("delete/users")]
        [Authorize(Roles = "Admin")]
        public async Task DeleteUser(string? email, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteUserCommand(email), cancellationToken);
        }
    }
}
