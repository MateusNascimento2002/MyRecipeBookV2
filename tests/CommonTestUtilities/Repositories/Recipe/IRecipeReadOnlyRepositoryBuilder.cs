using Moq;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Interfaces.Repositories.Recipe;

namespace CommonTestUtilities.Repositories.Recipe;

public class IRecipeReadOnlyRepositoryBuilder
{
    private readonly Mock<IRecipeReadOnlyRepository> _recipeReadOnlyRepositoryMock;

    public IRecipeReadOnlyRepositoryBuilder()
    {
        _recipeReadOnlyRepositoryMock = new Mock<IRecipeReadOnlyRepository>();
    }

    public IRecipeReadOnlyRepositoryBuilder GetById(MyRecipeBook.Domain.Entities.Recipe recipe)
    {
        _recipeReadOnlyRepositoryMock.Setup(repo => repo.GetById(recipe.Id, recipe.UserId)).ReturnsAsync(recipe);
        return this;
    }

    public IRecipeReadOnlyRepositoryBuilder GetRecentRecipes(MyRecipeBook.Domain.Entities.User user,
        IList<MyRecipeBook.Domain.Entities.Recipe> recipes)
    {
        var summaryRecipes = recipes.Select(r => new RecipeSummaryDto(r.Id, r.Title)).ToList();
        
        _recipeReadOnlyRepositoryMock.Setup(repo => repo.GetRecentRecipes(user.Id)).ReturnsAsync(summaryRecipes);
        
        return this;
    }

    public IRecipeReadOnlyRepositoryBuilder FilterRecipes(MyRecipeBook.Domain.Entities.User user,
        RecipeFilterDto expectedFilter, IList<MyRecipeBook.Domain.Entities.Recipe> recipes)
    {
        var summaryRecipes = recipes.Select(r => new RecipeSummaryDto(r.Id, r.Title)).ToList();

        _recipeReadOnlyRepositoryMock
            .Setup(repo => repo.FilterRecipes(user.Id, It.Is<RecipeFilterDto>(filter =>
                filter.SearchTerm == expectedFilter.SearchTerm &&
                filter.CookTime == expectedFilter.CookTime &&
                filter.DishTypes.SequenceEqual(expectedFilter.DishTypes))))
            .ReturnsAsync(summaryRecipes);

        return this;
    }

    public IRecipeReadOnlyRepository Build() => _recipeReadOnlyRepositoryMock.Object;
}