using FluentValidation;
using SniffAndStay.Application.Authentication.Commands;

namespace SniffAndStay.Application.Authentication.Validators
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(100);

            RuleFor(x => x.Password)
                .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.");

            RuleFor(x => x.Password)
                .Matches("[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.");

            RuleFor(x => x.Password)
                .Matches(@"\d")
                .WithMessage("Password must contain at least one digit.");

            RuleFor(x => x.Password)
                .Matches(@"[!@#$%^&*()_\-+=\[{\]};:'"",.<>/?\\|`~]")
                .WithMessage("Password must contain at least one special character.");
        }
    }
}
