using System.ComponentModel;

namespace SniffAndStay.Domain.Entities
{
    public enum PetType
    {
        Unknown = 0,
        [Description("Dog")]
        Dog = 1,
        Cat = 2,
        Other = 3
    }

    public class Pet
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public string? Picture { get; set; }
        public PetType PetType { get; set; } = PetType.Unknown;

        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
    }
}
