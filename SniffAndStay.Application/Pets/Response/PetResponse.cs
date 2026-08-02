using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Pets.Response
{
    public record PetResponse(string Name, DateOnly DateOfBirth, PetType PetType, string? ProfilePicture, string OwnerId)
    {
        public static PetResponse FromPet(Pet pet)
        {
            return new PetResponse(
                Name: pet.Name,
                DateOfBirth: pet.DateOfBirth,
                PetType: pet.PetType,
                ProfilePicture: pet.Picture,
                OwnerId: pet.UserId.ToString()
            );
        }
    }
}
