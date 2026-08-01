namespace SniffAndStay.Domain.Entities
{
    public enum PetType
    {
        Unknown = 0,
        Dog = 1,
        Cat = 2,
        Other = 3
    }

    public class Pet
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Picture { get; set; }
        public PetType Type { get; set; } = PetType.Unknown;

        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
    }
}
