using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exception;

namespace MyRecipeBook.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
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
            .WithMessage(ResourceMessagesException.VALIDATION_EMAIL_MAX_LENGTH)
            .EmailAddress()
            .WithMessage(ResourceMessagesException.VALIDATION_EMAIL_INVALID);
    }
}