using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.User;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using MyRecipeBook.Application.UseCases.Login.WithEmailAndPassword;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using Shouldly;
using DomainUser = MyRecipeBook.Domain.Entities.User;

namespace UseCases.Tests.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();

        request.Email = user.Email;

        var useCase = CreateUseCase(request.Password, user);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Tokens.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Tokens.AccessToken.ShouldNotBeNullOrEmpty();
        result.Tokens.RefreshToken.ShouldBeNullOrEmpty();
    }

    [Fact]
    public async Task ShouldThrowException_WhenUserDontExist()
    {
        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidLoginException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.Unauthorized);
        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_LOGIN_INVALID);
        });
    }

    [Fact]
    public async Task ShouldThrowException_WhenPasswordIsWrong()
    {
        var request = RequestLoginJsonBuilder.Build();
        var (user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(string.Empty, user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidLoginException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.Unauthorized);
        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_LOGIN_INVALID);
        });
    }

    private LoginWithEmailAndPasswordUseCase CreateUseCase(string? password = null, DomainUser? user = null)
    {
        var passwordHasher = new IPasswordHasherBuilder();
        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
        var accessTokenGenerator = IAccessTokenGeneratorBuilder.Build();

        if (user is not null)
            userReadOnlyRepositoryBuilder.GetUserByEmail(user);

        if (password.IsNotEmpty())
            passwordHasher.VerifyHashedPassword(password);

        return new LoginWithEmailAndPasswordUseCase(passwordHasher.Build(), userReadOnlyRepositoryBuilder.Build(),
            accessTokenGenerator);
    }
}