using Microsoft.EntityFrameworkCore;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    }
}
