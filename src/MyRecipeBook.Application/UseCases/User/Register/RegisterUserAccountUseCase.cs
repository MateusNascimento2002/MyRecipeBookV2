using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Exception.ExceptionBase;
using DomainUser = MyRecipeBook.Domain.Entities.User;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserAccountUseCase : IRegisterUserAccountUseCase
{
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserAccountUseCase(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }
    
    public void Execute(RequestRegisterUserAccountJson request)
    {
        ValidateAndThrowOnFailure(request);
        
        var password = _passwordHasher.HashPassword(request.Password);
        
        var user = DomainUser.Create(request.Name, request.Email, password);
    }

    private void ValidateAndThrowOnFailure(RequestRegisterUserAccountJson request)
    {
        var validator = new RegisterUserAccountValidator();
        var result = validator.Validate(request);

        if (result.IsValid) return;

        var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}