namespace SniffAndStay.Application.Authentication.DTOs
{
    /// <summary>
    /// This record represents a registration request containing the user's email and password.
    /// </summary>
    /// <param name="Email">The user's email address.</param>
    /// <param name="Password">The user's password.</param>
    public record RegisterRequest(string Email, string Password);
}
