using FluentValidation;
using SniffAndStay.Application.Common.Validators;
using SniffAndStay.Application.Pets.Commands;

namespace SniffAndStay.Application.Pets.Validators
{
    public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
    {
        public AddPetCommandValidator()
        {
            RuleFor(pet => pet.FileName).MustBeValidImageFileName();
            RuleFor(pet => pet.ContentType).MustBeValidImageContentType();
            RuleFor(pet => pet.File).MustNotExceedMaxImageSize();

            RuleFor(pet => pet.PetRequest.Name)
                .NotEmpty()
                .WithMessage("Pet name is required.")
                .MaximumLength(50)
                .WithMessage("Pet name is too long.");

            RuleFor(pet => pet.PetRequest.DateOfBirth)
                .NotEmpty()
                .WithMessage("Pet date of birth is required.")
                .GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30)))
                .WithMessage("Pet date of birth is too far in the past.")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Pet date of birth is in the future.");
        }
    }
}
