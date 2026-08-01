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
            bool doesUserExist = await _applicationDbContext.Users
                .AnyAsync(u => string.Equals(u.Email.ToLower(), request.Email.ToLower()), cancellationToken);
            if (doesUserExist)
            {
                throw new InvalidUserException($"User with email {request.Email} already exists.");
            }

            User newUser = new()
            {
                Email = request.Email,
                PasswordHash = _passwordHasherService.Hash(request.Password)
            };

            await _applicationDbContext.Users.AddAsync(newUser, cancellationToken);

            string activeToken = _jwtService.GenerateToken(TokenClaims.ToTokenClaim(newUser));
            string refreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenEntity = Domain.Entities.RefreshToken.CreateResfreshToken(newUser.Id, refreshToken, Constants.RefreshTokenExpirationDays);

            await _applicationDbContext.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new AuthenticationResponse(newUser.Id, activeToken, refreshToken);
        }
    }
}
