using SniffAndStay.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace SniffAndStay.Application.Pets.DTOs
{
    /// <summary>
    /// This record represents a request to create or update a pet, containing the pet's name, date of birth, and type.
    /// </summary>
    /// <param name="Name">Pet name</param>
    /// <param name="DateOfBirth">Date of birth</param>
    /// <param name="PetType">Type of pet</param>
    public record PetRequest([Required] string Name, [Required] DateOnly DateOfBirth, [Required] PetType PetType);
}
