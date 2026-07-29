using Microsoft.EntityFrameworkCore;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users => Set<User>();
    }
}
