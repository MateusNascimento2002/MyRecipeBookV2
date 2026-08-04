using Moq;
using MyRecipeBook.Domain.Interfaces.Repositories.Recipe;

namespace CommonTestUtilities.Repositories.Recipe;

public class IRecipeUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IRecipeUpdateOnlyRepository> _recipeUpdateOnlyRepositoryMock;

    public IRecipeUpdateOnlyRepositoryBuilder()
    {
        _recipeUpdateOnlyRepositoryMock = new Mock<IRecipeUpdateOnlyRepository>();
    }

    public IRecipeUpdateOnlyRepositoryBuilder GetById(MyRecipeBook.Domain.Entities.Recipe recipe)
    {
        _recipeUpdateOnlyRepositoryMock.Setup(repo => repo.GetById(recipe.Id, recipe.UserId)).ReturnsAsync(recipe);
        return this;
    }

    public IRecipeUpdateOnlyRepository Build() => _recipeUpdateOnlyRepositoryMock.Object;
}
