using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Common.Security
{
    public class TokenClaims
    {
        public Guid UserId { get; }
        public string Email { get; }
        public UserRole Role { get; }

        private TokenClaims(Guid userId, string email, UserRole role)
        {
            UserId = userId;
            Email = email;
            Role = role;
        }

        public static TokenClaims ToTokenClaim(User user)
        {
            return new TokenClaims(user.Id, user.Email, user.Role);
        }
    }
}
