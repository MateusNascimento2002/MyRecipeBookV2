using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Interfaces.Repositories.Users;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Exception.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCase : ILoginWithEmailAndPasswordUseCase
{
    private readonly IPasswordHasher _passwordHasher;
    private IUserReadOnlyRepository _userReadOnlyRepository;

    public LoginWithEmailAndPasswordUseCase(IPasswordHasher passwordHasher, IUserReadOnlyRepository userReadOnlyRepository)
    {
        _passwordHasher = passwordHasher;
        _userReadOnlyRepository = userReadOnlyRepository;
    }
    
    public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
    {
        var user = await _userReadOnlyRepository.GetByEmail(request.Email);

        if (user is null)
            throw new InvalidLoginException();
        
        var isPasswordValid = _passwordHasher.VerifyHashedPassword(request.Password, user.Password);

        if (isPasswordValid == false)
            throw new InvalidLoginException();

        return new ResponseRegisterUserJson(user.Id, user.Name, new ResponseTokensJson("access", "refresh"));
    }
}