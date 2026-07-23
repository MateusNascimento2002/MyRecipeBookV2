using FluentValidation.Results;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using DomainUser = MyRecipeBook.Domain.Entities.User;
namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IPasswordHasher _passwordHasher;

    public ChangePasswordUseCase(ILoggedUser loggedUser, IPasswordHasher passwordHasher)
    {
        _loggedUser = loggedUser;
        _passwordHasher = passwordHasher;
    }
    
    public async Task Execute(RequestChangePasswordJson request)
    {
        var user = await _loggedUser.Get();
        Validate(request, user);
        user.Password = _passwordHasher.HashPassword(request.NewPassword);
    }

    private void Validate(RequestChangePasswordJson request, DomainUser user)
    {
        var result = new ChangePasswordValidator().Validate(request);
        var isPasswordValid = _passwordHasher.VerifyPassword(request.CurrentPassword, user.Password);
         
        if(isPasswordValid == false)
            result.Errors.Add(new ValidationFailure("CurrentPassword", ResourceMessagesException.VALIDATION_CURRENT_PASSWORD_INVALID));
        
        if (result.IsValid == false)
            throw new ErrorOnValidationException(result.Errors.Select(x => x.ErrorMessage).ToList());
    }
}