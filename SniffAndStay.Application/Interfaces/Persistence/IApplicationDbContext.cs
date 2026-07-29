using Microsoft.EntityFrameworkCore;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Interfaces.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
