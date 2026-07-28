using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Recipe;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.Mappings;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using Shouldly;
using DomainUser = MyRecipeBook.Domain.Entities.User;

namespace UseCases.Tests.Recipe.Register;

public class RegisterRecipeUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestRecipeJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.Title.ShouldBe(request.Title);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenTitleIsEmpty()
    {
        var (user, _) = UserBuilder.Build();

        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_RECIPE_TITLE_REQUIRED);
        });
    }

    private static RegisterRecipeUseCase CreateUseCase(DomainUser user)
    {
        MapsterConfiguration.Configure();

        var recipeWriteOnlyRepository = IRecipeWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = IUnitOfWorkBuilder.Build();
        var loggedUser = ILoggedUserBuilder.Build(user);

        return new RegisterRecipeUseCase(recipeWriteOnlyRepository, unitOfWork, loggedUser);
    }
}
