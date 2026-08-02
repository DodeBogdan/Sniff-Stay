using Microsoft.EntityFrameworkCore;
using SniffAndStay.Application.Common.Extensions;
using SniffAndStay.Application.Exceptions;
using SniffAndStay.Application.Interfaces.Common;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Domain.Entities;
using SniffAndStay.Infrastructure.Persistence.Extensions;

namespace SniffAndStay.Infrastructure.Common
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public UserService(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<User> GetUserAsync(Guid? userId, IncludeType includeType = IncludeType.None, CancellationToken cancellationToken = default)
        {
            if(userId.IsNullOrEmpty())
            {
                throw new InvalidUserException("Invalid user");
            }

            User user = await _applicationDbContext.Users
                .AsNoTracking()
                .AddInclude(includeType)
                .FirstOrDefaultAsync(user => Guid.Equals(user.Id, userId), cancellationToken)
                ?? throw new InvalidUserException("Invalid user");

            return user;
        }
    }
}
