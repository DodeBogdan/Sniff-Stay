using MediatR;
using Microsoft.EntityFrameworkCore;
using SniffAndStay.Application.Authentication.Response;
using SniffAndStay.Application.Exceptions;
using SniffAndStay.Application.Interfaces;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SniffAndStay.Application.Authentication.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<AuthenticationResponse>;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(
            IApplicationDbContext context,
            IPasswordHasherService passwordHasherService,
            IJwtService jwtService)
        {
            _context = context;
            _passwordHasherService = passwordHasherService;
            _jwtService = jwtService;
        }

        public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email, cancellationToken)
                ?? throw new InvalidUserException($"User with email: {request.Email} not found.");

            if(!_passwordHasherService.Verify(request.Password, user.PasswordHash))
            {
                throw new InvalidUserException($"Invalid password for user: {request.Email}");
            }

            string token = _jwtService.GenerateToken(user.Id, user.Email);
            return new AuthenticationResponse(token);
        }
    }
}
