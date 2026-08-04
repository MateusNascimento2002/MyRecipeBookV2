using Moq;
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
        _recipeReadOnlyRepositoryMock.Setup(repo => repo.GetRecentRecipes(user.Id)).ReturnsAsync(recipes);
        return this;
    }

    public IRecipeReadOnlyRepository Build() => _recipeReadOnlyRepositoryMock.Object;
}