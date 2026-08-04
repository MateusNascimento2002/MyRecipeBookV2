using Moq;
using MyRecipeBook.Domain.Interfaces.Repositories.Recipe;

namespace CommonTestUtilities.Repositories.Recipe;

public class IRecipeWriteOnlyRepositoryBuilder
{
    private readonly Mock<IRecipeWriteOnlyRepository> _recipeWriteOnlyRepositoryMock;
    public IRecipeWriteOnlyRepositoryBuilder()
    {
        _recipeWriteOnlyRepositoryMock = new Mock<IRecipeWriteOnlyRepository>();
    }
    
    public IRecipeWriteOnlyRepositoryBuilder DeleteById(MyRecipeBook.Domain.Entities.Recipe recipe)
    {
        _recipeWriteOnlyRepositoryMock.Setup(repo => repo.DeleteById(recipe.Id, recipe.UserId)).ReturnsAsync(true);
        return this;
    }
    
    public IRecipeWriteOnlyRepository Build() => _recipeWriteOnlyRepositoryMock.Object;
}