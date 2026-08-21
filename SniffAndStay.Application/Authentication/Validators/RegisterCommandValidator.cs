using FluentValidation;
using SniffAndStay.Application.Authentication.Commands;

namespace SniffAndStay.Application.Authentication.Validators
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private const int EmailMaxLength = 255;
        private const int PasswordMaxLength = 100;
        private const int PasswordMinLength = 8;


        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .MaximumLength(EmailMaxLength)
                .WithMessage("Email must not exceed 255 characters.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(PasswordMinLength)
                .WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(PasswordMaxLength)
                .WithMessage("Password must not exceed 100 characters.")
                .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d")
                .WithMessage("Password must contain at least one digit.")
                .Matches(@"[!@#$%^&*()_\-+=\[{\]};:'"",.<>/?\\|`~]")
                .WithMessage("Password must contain at least one special character.");
        }
    }
}
