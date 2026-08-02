using Microsoft.EntityFrameworkCore;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Infrastructure.Persistence.Extensions
{
    public static class QueryExtensions
    {
        public static IQueryable<User> AddInclude(this IQueryable<User> query, IncludeType includeType)
        {
            return includeType switch
            {
                IncludeType.None => query,
                IncludeType.Details => query.Include(user => user.UserDetails),
                IncludeType.Pets => query.Include(user => user.Pets),
                IncludeType.All => query.Include(user => user.UserDetails)
                                        .Include(user => user.Pets),
                _ => throw new ArgumentOutOfRangeException(nameof(includeType), includeType, null)
            };
        }
    }
}
