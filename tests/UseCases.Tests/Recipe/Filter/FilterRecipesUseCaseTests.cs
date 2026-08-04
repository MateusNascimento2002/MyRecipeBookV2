using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories.Recipe;
using MyRecipeBook.Application.Mappings;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Communication.Enums.Recipe;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Dtos;
using Shouldly;
using DomainCookTime = MyRecipeBook.Domain.Enums.Recipe.CookTime;
using DomainDishType = MyRecipeBook.Domain.Enums.Recipe.DishType;
using DomainRecipe = MyRecipeBook.Domain.Entities.Recipe;
using DomainUser = MyRecipeBook.Domain.Entities.User;

namespace UseCases.Tests.Recipe.Filter;

public class FilterRecipesUseCaseTests
{
    [Fact]
    public async Task Success_ShouldNotFilter_WhenRequestIsNull()
    {
        var (user, _) = UserBuilder.Build();
        var recipes = Enumerable.Range(0, 3).Select(_ => RecipeBuilder.Build(user)).ToList();

        var useCase = CreateUseCase(user, new RecipeFilterDto(), recipes);

        var result = await useCase.Execute(null);

        result.ShouldNotBeNull();
        result.Recipes.ShouldSatisfyAllConditions(recipeSummaries =>
        {
            recipeSummaries.Count.ShouldBe(recipes.Count);
            recipeSummaries.Select(recipe => recipe.Id).ShouldBe(recipes.Select(recipe => recipe.Id));
            recipeSummaries.Select(recipe => recipe.Title).ShouldBe(recipes.Select(recipe => recipe.Title));
        });
    }

    [Fact]
    public async Task Success_ShouldNotFilter_WhenRequestIsEmpty()
    {
        var (user, _) = UserBuilder.Build();
        var recipes = Enumerable.Range(0, 3).Select(_ => RecipeBuilder.Build(user)).ToList();

        var useCase = CreateUseCase(user, new RecipeFilterDto(), recipes);

        var result = await useCase.Execute(new RequestFilterRecipesJson());

        result.ShouldNotBeNull();
        result.Recipes.ShouldSatisfyAllConditions(recipeSummaries =>
        {
            recipeSummaries.Count.ShouldBe(recipes.Count);
            recipeSummaries.Select(recipe => recipe.Id).ShouldBe(recipes.Select(recipe => recipe.Id));
            recipeSummaries.Select(recipe => recipe.Title).ShouldBe(recipes.Select(recipe => recipe.Title));
        });
    }

    [Fact]
    public async Task Success_ShouldFilter_WhenRequestHasFilters()
    {
        var (user, _) = UserBuilder.Build();
        var recipes = new List<DomainRecipe> { RecipeBuilder.Build(user) };

        var request = new RequestFilterRecipesJson
        {
            SearchTerm = "chocolate",
            CookTime = CookTime.From30To60Minutes,
            DishTypes = [DishType.Dessert]
        };

        var expectedFilter = new RecipeFilterDto
        {
            SearchTerm = "chocolate",
            CookTime = DomainCookTime.From30To60Minutes,
            DishTypes = [DomainDishType.Dessert]
        };

        var useCase = CreateUseCase(user, expectedFilter, recipes);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Recipes.ShouldSatisfyAllConditions(recipeSummaries =>
        {
            recipeSummaries.ShouldHaveSingleItem();
            recipeSummaries.Select(recipe => recipe.Id).ShouldBe(recipes.Select(recipe => recipe.Id));
        });
    }

    private static FilterRecipesUseCase CreateUseCase(DomainUser user, RecipeFilterDto expectedFilter,
        IList<DomainRecipe> recipes)
    {
        MapsterConfiguration.Configure();
        var loggedUser = ILoggedUserBuilder.Build(user);
        var repository = new IRecipeReadOnlyRepositoryBuilder().FilterRecipes(user, expectedFilter, recipes).Build();
        return new FilterRecipesUseCase(repository, loggedUser);
    }
}
