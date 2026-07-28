using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories.Recipe;
using MyRecipeBook.Application.Mappings;
using MyRecipeBook.Application.UseCases.Recipe.GetById;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using Shouldly;

namespace UseCases.Tests.Recipe.GetById;

public class GetRecipeByIdUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        
        var useCase = CreateUseCase(recipe, user);
        
        var result = await useCase.Execute(recipe.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(recipe.Id);
        result.Title.ShouldBe(recipe.Title);
        
        result.Instructions.Select(c => c.Order).ShouldBeInOrder(SortDirection.Ascending);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenRecipeNotFound()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        
        var useCase = CreateUseCase(recipe, user);
        
        var exception = await useCase.Execute(Guid.CreateVersion7()).ShouldThrowAsync<NotFoundException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);
        });
    }
    
    private static GetRecipeByIdUseCase CreateUseCase(MyRecipeBook.Domain.Entities.Recipe recipe, MyRecipeBook.Domain.Entities.User user)
    {
        MapsterConfiguration.Configure();
        var loggedUser = ILoggedUserBuilder.Build(user);
        var repository = new IRecipeReadOnlyRepositoryBuilder().GetById(recipe).Build();
        return new GetRecipeByIdUseCase(repository, loggedUser);
    }
}