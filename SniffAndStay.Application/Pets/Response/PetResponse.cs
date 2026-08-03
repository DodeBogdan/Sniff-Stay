using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Pets.Response
{
    public record PetResponse(Guid PetId, string Name, DateOnly DateOfBirth, PetType PetType, string? ProfilePicture, Guid OwnerId)
    {
        public static PetResponse FromPet(Pet pet)
        {
            return new PetResponse(
                PetId: pet.Id,
                Name: pet.Name,
                DateOfBirth: pet.DateOfBirth,
                PetType: pet.PetType,
                ProfilePicture: pet.Picture,
                OwnerId: pet.UserId
            );
        } 
    }
}
