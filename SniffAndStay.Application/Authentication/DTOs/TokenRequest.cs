namespace SniffAndStay.Application.Authentication.DTOs
{
    /// <summary>
    /// This record represents a token request containing the user's ID and refresh token.
    /// </summary>
    /// <param name="UserId">The user's ID.</param>
    /// <param name="RefreshToken">The refresh token.</param>
    public record TokenRequest(Guid UserId, string RefreshToken);
}
