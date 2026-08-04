using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Recipe;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.Mappings;
using MyRecipeBook.Application.UseCases.Recipe.Update;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using Shouldly;

namespace UseCases.Tests.Recipe.Update;

public class UpdateRecipeByIdUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var request = RequestRecipeJsonBuilder.Build();

        var useCase = CreateUseCase(recipe, user);

        await useCase.Execute(recipe.Id, request).ShouldNotThrowAsync();

        recipe.ShouldSatisfyAllConditions(updatedRecipe =>
        {
            updatedRecipe.Title.ShouldBe(request.Title);
            updatedRecipe.Description.ShouldBe(request.Description);
            updatedRecipe.CookTime.ToString().ShouldBe(request.CookTime.ToString());
            updatedRecipe.Ingredients.Select(ingredient => ingredient.Item).ShouldBe(request.Ingredients, true);
            updatedRecipe.Instructions.Select(instruction => instruction.Description)
                .ShouldBe(request.Instructions.Select(instruction => instruction.Description), true);
            updatedRecipe.DishTypes.Select(dishType => dishType.Type.ToString())
                .ShouldBe(request.DishTypes.Select(dishType => dishType.ToString()), true);
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenTitleIsEmpty()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(recipe, user);

        var exception = await useCase.Execute(recipe.Id, request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);
        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_RECIPE_TITLE_REQUIRED);
        });

        recipe.Title.ShouldNotBe(request.Title);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenRecipeNotFound()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var request = RequestRecipeJsonBuilder.Build();

        var useCase = CreateUseCase(recipe, user);

        var exception = await useCase.Execute(Guid.CreateVersion7(), request).ShouldThrowAsync<NotFoundException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_RECIPE_NOT_FOUND);
        });

        recipe.Title.ShouldNotBe(request.Title);
    }

    private static UpdateRecipeByIdUseCase CreateUseCase(MyRecipeBook.Domain.Entities.Recipe recipe,
        MyRecipeBook.Domain.Entities.User user)
    {
        MapsterConfiguration.Configure();
        var loggedUser = ILoggedUserBuilder.Build(user);
        var unitOfWork = IUnitOfWorkBuilder.Build();
        var repository = new IRecipeUpdateOnlyRepositoryBuilder().GetById(recipe).Build();
        return new UpdateRecipeByIdUseCase(repository, unitOfWork, loggedUser);
    }
}
