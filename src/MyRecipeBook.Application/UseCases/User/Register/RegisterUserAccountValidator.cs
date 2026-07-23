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
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED);

        RuleFor(x => x.Password).Password();

        When(user => user.Email.IsNotEmpty(), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage(ResourceMessagesException.VALIDATION_EMAIL_INVALID);
        });
    }
}