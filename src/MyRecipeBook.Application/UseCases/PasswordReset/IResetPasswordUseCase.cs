using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.PasswordReset;

public interface IResetPasswordUseCase
{
    Task Execute(RequestResetPasswordJson request);
}