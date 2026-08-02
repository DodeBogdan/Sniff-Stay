using SniffAndStay.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace SniffAndStay.Application.Pets.DTOs
{
    public record PetRequest([Required] string Name, [Required] DateOnly DateOfBirth, [Required] PetType PetType);
}
