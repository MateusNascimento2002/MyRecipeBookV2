using FluentValidation.Results;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Interfaces.Repositories.UnitOfWork;
using MyRecipeBook.Domain.Interfaces.Repositories.Users;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using DomainUser = MyRecipeBook.Domain.Entities.User;

namespace MyRecipeBook.Application.UseCases.User.Update;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;

    public UpdateUserUseCase(ILoggedUser loggedUser, IUserReadOnlyRepository userReadOnlyRepository,
        IUserUpdateOnlyRepository userUpdateOnlyRepository, IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestUpdateUserJson request)
    {
        var user = await _loggedUser.Get();
        await Validate(request, user);
        user.Email = request.Email;
        user.Name = request.Name;
        _userUpdateOnlyRepository.UpdateProfile(user);
        await _unitOfWork.CommitAsync();
    }

    private async Task Validate(RequestUpdateUserJson request, DomainUser user)
    {
        var result = await new UpdateUserValidator().ValidateAsync(request);

        if (user.Email.Equals(request.Email) == false)
        {
            var emailAlreadyInUse = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if (emailAlreadyInUse)
                result.Errors.Add(new ValidationFailure("Email",
                    ResourceMessagesException.VALIDATION_EMAIL_ALREADY_EXISTS));
        }

        if (!result.IsValid) throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}