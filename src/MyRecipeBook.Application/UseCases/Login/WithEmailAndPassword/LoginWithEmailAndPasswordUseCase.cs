using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Interfaces.Repositories.Users;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exception.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCase : ILoginWithEmailAndPasswordUseCase
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public LoginWithEmailAndPasswordUseCase(IPasswordHasher passwordHasher,
        IUserReadOnlyRepository userReadOnlyRepository, IAccessTokenGenerator accessTokenGenerator)
    {
        _passwordHasher = passwordHasher;
        _userReadOnlyRepository = userReadOnlyRepository;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
    {
        var user = await _userReadOnlyRepository.GetByEmail(request.Email);

        if (user is null)
            throw new InvalidLoginException();

        var isPasswordValid = _passwordHasher.VerifyHashedPassword(request.Password, user.Password);

        if (isPasswordValid == false)
            throw new InvalidLoginException();

        return new ResponseRegisterUserJson(user.Id, user.Name, new ResponseTokensJson(_accessTokenGenerator.Generate(user), null));
    }
}