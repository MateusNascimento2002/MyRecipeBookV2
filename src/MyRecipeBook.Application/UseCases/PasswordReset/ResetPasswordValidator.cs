using FluentValidation;
using MyRecipeBook.Application.Shared.Validators;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.PasswordReset;

public class ResetPasswordValidator : AbstractValidator<RequestResetPasswordJson>
{
    public ResetPasswordValidator()
    {
        RuleFor(request => request.NewPassword).Password();
    }
}