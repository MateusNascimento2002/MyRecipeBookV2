using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.PasswordRecovery;

public interface IRequestPasswordRecoveryCodeUseCase
{
    Task Execute(RequestPasswordRecoveryJson request);
}