using MediatR;
using Microsoft.EntityFrameworkCore;
using SniffAndStay.Application.Authentication.Response;
using SniffAndStay.Application.Common.Constants;
using SniffAndStay.Application.Common.Security;
using SniffAndStay.Application.Exceptions;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Authentication.Commands
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
            User user = await _context.Users.SingleOrDefaultAsync(u => string.Equals(u.Email.ToLower(), request.Email.ToLower()), cancellationToken)
                ?? throw new InvalidUserException($"User with email: {request.Email} not found.");

            if(!_passwordHasherService.Verify(request.Password, user.PasswordHash))
            {
                throw new InvalidUserException($"Invalid password for user: {request.Email}");
            }

            string activeToken = _jwtService.GenerateToken(TokenClaims.ToTokenClaim(user));
            string refreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenEntity = Domain.Entities.RefreshToken.CreateResfreshToken(user.Id, refreshToken, Constants.RefreshTokenExpirationDays);

            await _context.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new AuthenticationResponse(user.Id, activeToken, refreshToken);
        }
    }
}
