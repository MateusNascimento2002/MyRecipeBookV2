using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using MyRecipeBook.Application.UseCases.User.Profile;
using Shouldly;
using DomainUser = MyRecipeBook.Domain.Entities.User;

namespace UseCases.Tests.User.Profile;

public class GetUserProfileUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();

        var useCase = CreateUserCase(user);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Email.ShouldBe(user.Email);
    }

    private static GetUserProfileUseCase CreateUserCase(DomainUser user)
    {
        var loggedUser = ILoggedUserBuilder.Build(user);
        return new GetUserProfileUseCase(loggedUser);
    }
}