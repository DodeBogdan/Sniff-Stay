using MediatR;
using Microsoft.AspNetCore.Mvc;
using SniffAndStay.Application.Authentication.Commands.Login;
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
        public async Task<AuthenticationResponse> Login(LoginRequest request)
        {
            return await _mediator.Send(new LoginCommand(request.Email, request.Password));
        }

        [HttpPost("register")]
        public async Task<AuthenticationResponse> Register(RegisterRequest request)
        {
            return await _mediator.Send(new RegisterCommand(request.Email, request.Password));
        }
    }
}
