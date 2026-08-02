namespace SniffAndStay.Domain.Entities
{
    public enum Gender
    {
        Unknown = 0,
        Male = 1,
        Female = 2,
        Other = 3
    }

    public class UserDetails
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        public string LastName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public string? ProfilePicture { get; set; }
        public Gender Gender { get; set; } = Gender.Unknown;
    }
}
