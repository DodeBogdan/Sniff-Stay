using SniffAndStay.Application.Common.Security;

namespace SniffAndStay.Application.Interfaces.Security
{
    public interface IJwtService
    {
        string GenerateToken(TokenClaims tokenClaims);

        string GenerateRefreshToken();
    }
}
