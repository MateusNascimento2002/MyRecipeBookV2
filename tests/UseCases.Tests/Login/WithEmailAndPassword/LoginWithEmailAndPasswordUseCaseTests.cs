using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Login.WithEmailAndPassword;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using Shouldly;
using DomainUser = MyRecipeBook.Domain.Entities.User;
namespace UseCases.Tests.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();

        request.Email = user.Email;
        
        var useCase = CreateUseCase(password: request.Password, user: user);
        
        var result =  await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Tokens.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Tokens.AccessToken.ShouldBeNullOrEmpty();
        result.Tokens.RefreshToken.ShouldBeNullOrEmpty();
    }

    private LoginWithEmailAndPasswordUseCase CreateUseCase(string? password = null, DomainUser? user = null)
    {
        var passwordHasher = new IPasswordHasherBuilder();
        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();

        if (user is not null)
            userReadOnlyRepositoryBuilder.GetUserByEmail(user);

        if (password.IsNotEmpty())
            passwordHasher.VerifyHashedPassword(password);
        
        return new LoginWithEmailAndPasswordUseCase(passwordHasher.Build(), userReadOnlyRepositoryBuilder.Build());
    }
}