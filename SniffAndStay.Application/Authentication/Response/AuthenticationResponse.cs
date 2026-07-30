namespace SniffAndStay.Application.Authentication.Response
{
    public record AuthenticationResponse(Guid UserId, string ActiveToken, string RefreshToken);
}
