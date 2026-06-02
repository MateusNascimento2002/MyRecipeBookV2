using FluentValidation;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserAccountValidator : AbstractValidator<RequestRegisterUserAccountJson>
{
    public RegisterUserAccountValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);

        When(user => !string.IsNullOrEmpty(user.Email), () =>
        {
            RuleFor(x => x.Email) 
                .EmailAddress()
                .WithMessage("Invalid email format.");
        });
    }
}