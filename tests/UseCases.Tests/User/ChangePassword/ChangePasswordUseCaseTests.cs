using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using Shouldly;
using DomainUser = MyRecipeBook.Domain.Entities.User;

namespace UseCases.Tests.User.ChangePassword;

public class ChangePasswordUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, password) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.CurrentPassword = password;

        var useCase = CreateUseCase(user, password);

        await useCase.Execute(request).ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenNewPasswordIsEmpty()
    {
        var (user, password) = UserBuilder.Build();

        var request = new RequestChangePasswordJson()
        {
            NewPassword = string.Empty,
            CurrentPassword = password
        };

        var useCase = CreateUseCase(user, password);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED);
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenCurrentPasswordDoesNotMatch()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user, "invalid password");

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_CURRENT_PASSWORD_INVALID);
        });
    }

    private static ChangePasswordUseCase CreateUseCase(DomainUser user, string password)
    {
        var userUpdateOnlyRepository = IUserUpdateOnlyRepositoryBuilder.Build();
        var loggedUser = ILoggedUserBuilder.Build(user);
        var passwordHasherBuilder = new IPasswordHasherBuilder().VerifyHashedPassword(password).Build();


        return new ChangePasswordUseCase(loggedUser, passwordHasherBuilder, userUpdateOnlyRepository);
    }
}