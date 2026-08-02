using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Users.Response
{
    public class UserProfileResponse
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public int Age { get => DateTime.UtcNow.Subtract(DateOfBirth.ToDateTime(TimeOnly.MinValue)).Days / 365; }
        public Gender Gender { get; set; }
        public string ProfilePictureUrl { get; set; } = default!;

        public static UserProfileResponse FromUserDetails(User user)
        {
            return new UserProfileResponse
            {
                UserId = user.Id,
                FirstName = user.UserDetails?.FirstName ?? string.Empty,
                LastName = user.UserDetails?.LastName ?? string.Empty,
                Email = user.Email,
                DateOfBirth = user.UserDetails?.DateOfBirth ?? DateOnly.MinValue,
                Gender = user.UserDetails?.Gender ?? Gender.Unknown,
                ProfilePictureUrl = user.UserDetails?.ProfilePicture ?? string.Empty
            };
        }
    }
}
