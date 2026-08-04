namespace MyRecipeBook.Domain.Interfaces.Repositories.Recipe;

public interface IRecipeWriteOnlyRepository
{
    Task Add(Entities.Recipe recipe);
    Task<bool> DeleteById(Guid id, Guid userId);
}