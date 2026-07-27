using FluentValidation;
using MyRecipeBook.Application.Shared.Validators;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserAccountValidator : AbstractValidator<RequestRegisterUserAccountJson>
{
    public RegisterUserAccountValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED)
            .MaximumLength(256)
            .WithMessage(ResourceMessagesException.VALIDATION_NAME_MAX_LENGTH);

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED)
            .MaximumLength(256)
            .WithMessage(ResourceMessagesException.VALIDATION_EMAIL_MAX_LENGTH);

        RuleFor(x => x.Password).Password();

        When(user => user.Email.IsNotEmpty(), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage(ResourceMessagesException.VALIDATION_EMAIL_INVALID);
        });
    }
}