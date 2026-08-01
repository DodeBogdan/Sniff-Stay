using MediatR;
using Microsoft.EntityFrameworkCore;
using SniffAndStay.Application.Interfaces.Persistence;

namespace SniffAndStay.Application.Authentication.Commands
{
    public record LogoutCommand(Guid UserId, string RefreshToken) : IRequest;
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public LogoutCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.RefreshToken refreshToken = await _applicationDbContext.RefreshTokens
                .SingleOrDefaultAsync(rt => string.Equals(rt.Token, request.RefreshToken), cancellationToken)
                ?? throw new UnauthorizedAccessException("Invalid refresh token.");

            if(refreshToken.UserId != request.UserId)
            {
                throw new UnauthorizedAccessException("Refresh token does not belong to the user.");
            }

            refreshToken.RevokedAt = DateTime.UtcNow;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
