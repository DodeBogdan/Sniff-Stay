using MediatR;
using SniffAndStay.Application.Authentication.Response;
using SniffAndStay.Application.Exceptions;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Authentication.Commands.Register
{
    public record RegisterCommand(string Email, string Password) : IRequest<AuthenticationResponse>;
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResponse>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IJwtService _jwtService;
        public RegisterCommandHandler(
            IApplicationDbContext applicationDbContext,
            IPasswordHasherService passwordHasherService,
            IJwtService jwtService)
        {
            _applicationDbContext = applicationDbContext;
            _passwordHasherService = passwordHasherService;
            _jwtService = jwtService;
        }

        public async Task<AuthenticationResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            bool doesUserExist = _applicationDbContext.Users.Any(u => u.Email == request.Email);
            if (doesUserExist)
            {
                throw new InvalidUserException($"User with email {request.Email} already exists.");
            }

            //TODO: Complete with all data necessary for user creation, like username, etc.
            User newUser = new()
            {
                Email = request.Email,
                PasswordHash = _passwordHasherService.Hash(request.Password)
            };

            await _applicationDbContext.Users.AddAsync(newUser, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            string token = _jwtService.GenerateToken(newUser.Id, newUser.Email);
            return new AuthenticationResponse(token);
        }
    }
}
