using FluentValidation;
using SniffAndStay.Application.Common.Validators;
using SniffAndStay.Application.Users.Commands;

namespace SniffAndStay.Application.Users.Validators
{
    public class AddOrUpdateUserProfileCommandValidator : AbstractValidator<AddOrUpdateUserProfileCommand>
    {
        public AddOrUpdateUserProfileCommandValidator()
        {
            RuleFor(user => user.FileName).MustBeValidImageFileName();
            RuleFor(user => user.ContentType).MustBeValidImageContentType();
            RuleFor(user => user.File).MustNotExceedMaxImageSize();

            RuleFor(user => user.UpdateProfileRequest.FirstName)
                .MaximumLength(100)
                .When(user => user.UpdateProfileRequest.FirstName is not null)
                .WithMessage("First name is too long.");

            RuleFor(user => user.UpdateProfileRequest.LastName)
                .MaximumLength(100)
                .When(user => user.UpdateProfileRequest.LastName is not null)
                .WithMessage("Last name is too long.");

            RuleFor(user => user.UpdateProfileRequest.DateOfBirth)
                .GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-120)))
                .WithMessage("Date of birth is too far in the past.")
                .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Date of birth is in the future.")
                .When(user => user.UpdateProfileRequest.DateOfBirth is not null);
        }
    }
}
