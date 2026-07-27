namespace MyRecipeBook.Domain.Interfaces.Repositories.Recipe;

public interface IRecipeWriteOnlyRepository
{
    Task Add(Entities.Recipe recipe);
}