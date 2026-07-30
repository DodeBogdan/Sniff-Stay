namespace SniffAndStay.Application.Authentication.DTOs
{
    public record TokenRequest(Guid UserId, string RefreshToken);
}
