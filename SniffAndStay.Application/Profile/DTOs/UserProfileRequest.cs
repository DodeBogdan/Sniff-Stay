using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Profile.DTOs
{
    public record UserProfileRequest(string? FirstName, string? LastName, DateOnly? DateOfBirth, Gender? Gender);
}
