using Microsoft.EntityFrameworkCore;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Interfaces.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<RefreshToken> RefreshTokens { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
