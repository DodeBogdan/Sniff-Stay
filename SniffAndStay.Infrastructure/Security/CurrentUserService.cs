using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SniffAndStay.Application.Exceptions;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Domain.Entities;
using SniffAndStay.Infrastructure.Persistence.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SniffAndStay.Infrastructure.Security
{

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationDbContext _applicationDbContext;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            IApplicationDbContext applicationDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _applicationDbContext = applicationDbContext;
        }

        public Guid? UserId
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?.User?
                    .FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                return Guid.TryParse(value, out var userId) ? userId : null;
            }
        }

        public string? Email =>
            _httpContextAccessor.HttpContext?.User?
                .FindFirst(JwtRegisteredClaimNames.Email)?.Value;

        public string? Role =>
            _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.Role)?.Value;

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public async Task<User> GetUserAsync(IncludeType includeType, CancellationToken cancellationToken = default)
        {
            Guid userId = UserId
                ?? throw new InvalidUserException("Invalid user");

            User user = await _applicationDbContext.Users
                .AsNoTracking()
                .AddInclude(includeType)
                .FirstOrDefaultAsync(user => Guid.Equals(user.Id, userId), cancellationToken)
                ?? throw new InvalidUserException("Invalid user");

            return user;
        }
    }
}
