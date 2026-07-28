namespace MyRecipeBook.Domain.Interfaces.Repositories.Recipe;

public interface IRecipeReadOnlyRepository
{
    Task<Entities.Recipe?> GetById(Guid id, Guid userId);
}