using MediatR;
using Microsoft.EntityFrameworkCore;
using SniffAndStay.Application.Authentication.Response;
using SniffAndStay.Application.Common.Constants;
using SniffAndStay.Application.Common.Security;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Authentication.Commands.RefreshToken
{
    public record RefreshTokenCommand(Guid UserId, string RefreshToken) : IRequest<AuthenticationResponse>;
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthenticationResponse>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IJwtService _jwtService;
        public RefreshTokenCommandHandler(IApplicationDbContext applicationDbContext, IJwtService jwtService)
        {
            _applicationDbContext = applicationDbContext;
            _jwtService = jwtService;
        }

        public async Task<AuthenticationResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.RefreshToken refreshToken = await _applicationDbContext.RefreshTokens
                .SingleOrDefaultAsync(rt => string.Equals(rt.Token, request.RefreshToken), cancellationToken)
                ?? throw new UnauthorizedAccessException("Invalid refresh token.");

            if (refreshToken.UserId != request.UserId)
            {
                throw new UnauthorizedAccessException("Refresh token does not belong to the user.");
            }

            if (!refreshToken.IsActive)
            {
                throw new UnauthorizedAccessException("Refresh token is not active.");
            }

            string newRefreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenEntity = Domain.Entities.RefreshToken.CreateResfreshToken(refreshToken.UserId, newRefreshToken, Constants.RefreshTokenExpirationDays);

            refreshToken.RevokedAt = DateTime.UtcNow;

            User user = await _applicationDbContext.Users.SingleOrDefaultAsync(u => Guid.Equals(u.Id, refreshToken.UserId), cancellationToken)
                    ?? throw new UnauthorizedAccessException("User not found.");

            string activeToken = _jwtService.GenerateToken(TokenClaims.ToTokenClaim(user));

            await _applicationDbContext.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new AuthenticationResponse(user.Id, activeToken, newRefreshToken);
        }
    }
}
