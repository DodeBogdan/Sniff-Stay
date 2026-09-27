using FluentValidation;
using SniffAndStay.Application.Pets.Commands;

namespace SniffAndStay.Application.Pets.Validators
{
    public class RemovePetCommandValidator : AbstractValidator<RemovePetCommand>
    {
        public RemovePetCommandValidator()
        {
            RuleFor(pet => pet.PetId)
                .NotEmpty().WithMessage("PetId is required.")
                .Must(pet => pet != Guid.Empty).WithMessage("PetId must be a valid GUID.");
        }
    }
}
