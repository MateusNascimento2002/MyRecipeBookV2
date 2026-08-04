using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories.Recipe;
using MyRecipeBook.Application.Mappings;
using MyRecipeBook.Application.UseCases.Recipe.GetRecent;
using Shouldly;

namespace UseCases.Tests.Recipe.GetRecent;

public class GetRecentRecipesUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var recipes = Enumerable.Range(0, 6).Select(_ => RecipeBuilder.Build(user)).ToList();

        var useCase = CreateUseCase(user, recipes);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Recipes.ShouldSatisfyAllConditions(recipeSummaries =>
        {
            recipeSummaries.Count.ShouldBe(recipes.Count);
            recipeSummaries.Select(recipe => recipe.Id).ShouldBe(recipes.Select(recipe => recipe.Id));
            recipeSummaries.Select(recipe => recipe.Title).ShouldBe(recipes.Select(recipe => recipe.Title));
        });
    }

    [Fact]
    public async Task Success_ShouldReturnEmptyList_WhenUserHasNoRecipes()
    {
        var (user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user, []);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Recipes.ShouldBeEmpty();
    }

    private static GetRecentRecipesUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user,
        IList<MyRecipeBook.Domain.Entities.Recipe> recipes)
    {
        MapsterConfiguration.Configure();
        var loggedUser = ILoggedUserBuilder.Build(user);
        var repository = new IRecipeReadOnlyRepositoryBuilder().GetRecentRecipes(user, recipes).Build();
        return new GetRecentRecipesUseCase(repository, loggedUser);
    }
}
