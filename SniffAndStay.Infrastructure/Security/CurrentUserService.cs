using Microsoft.AspNetCore.Http;
using SniffAndStay.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SniffAndStay.Infrastructure.Security
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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
    }
}
