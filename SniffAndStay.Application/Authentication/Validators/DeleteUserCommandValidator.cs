using FluentValidation;
using SniffAndStay.Application.Authentication.Commands;
using SniffAndStay.Application.Common.Extensions;

namespace SniffAndStay.Application.Authentication.Validators
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(user => user.Email)
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .When(user => !user.Email.IsNullOrEmpty());
        }
    }
}
