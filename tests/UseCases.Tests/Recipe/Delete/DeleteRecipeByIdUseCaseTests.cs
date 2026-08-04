using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories.Recipe;
using MyRecipeBook.Application.Mappings;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using Shouldly;

namespace UseCases.Tests.Recipe.Delete;

public class DeleteRecipeByIdUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        
        var useCase = CreateUseCase(recipe, user);
        
        await useCase.Execute(recipe.Id).ShouldNotThrowAsync();
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
    
    private static DeleteRecipeByIdUseCase CreateUseCase(MyRecipeBook.Domain.Entities.Recipe recipe, MyRecipeBook.Domain.Entities.User user)
    {
        MapsterConfiguration.Configure();
        var loggedUser = ILoggedUserBuilder.Build(user);
        var repository = new IRecipeWriteOnlyRepositoryBuilder().DeleteById(recipe).Build();
        return new DeleteRecipeByIdUseCase(repository, loggedUser);
    }
}