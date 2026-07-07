using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.User.Register;

public interface IRegisterUserAccountUseCase
{
    Task<ResponseRegisterUserJson> Execute(RequestRegisterUserAccountJson request);
}