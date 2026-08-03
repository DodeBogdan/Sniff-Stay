using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Users.DTOs
{
    /// <summary>
    /// This record represents a user profile request containing the user's first name, last name, date of birth, and gender.
    /// </summary>
    /// <param name="FirstName">The first name of the user</param>
    /// <param name="LastName">The last name of the user</param>
    /// <param name="DateOfBirth">The date of birth of the user</param>
    /// <param name="Gender">The gender of the user</param>
    public record UserProfileRequest(string? FirstName, string? LastName, DateOnly? DateOfBirth, Gender? Gender);
}
