using FluentValidation;
using SniffAndStay.Application.Common.Validators;
using SniffAndStay.Application.Pets.Commands;
using SniffAndStay.Domain.Constants;

namespace SniffAndStay.Application.Pets.Validators
{
    public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
    {
        private const int PetNameMaxLength = 50;
        private const int PetNameMinLength = 3;
        private readonly DateOnly PetMaximumBirthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30));
        private readonly DateOnly PetMinimumBirthDate = DateOnly.FromDateTime(DateTime.UtcNow);

        public AddPetCommandValidator()
        {
            RuleFor(pet => pet.FileName).MustBeValidImageFileName();
            RuleFor(pet => pet.ContentType).MustBeValidImageContentType();
            RuleFor(pet => pet.File).MustNotExceedMaxImageSize();

            RuleFor(pet => pet.PetRequest.Name)
                .NotEmpty()
                .WithMessage("Pet name is required.")
                .MaximumLength(PetNameMaxLength)
                .WithMessage("Pet name is too long.")
                .MinimumLength(PetNameMinLength)
                .WithMessage("Pet name is too short.");

            RuleFor(pet => pet.PetRequest.DateOfBirth)
                .NotEmpty()
                .WithMessage("Pet date of birth is required.")
                .GreaterThan(PetMaximumBirthDate)
                .WithMessage("Pet date of birth is too far in the past.")
                .LessThanOrEqualTo(PetMinimumBirthDate)
                .WithMessage("Pet date of birth is in the future.");
        }
    }
}
