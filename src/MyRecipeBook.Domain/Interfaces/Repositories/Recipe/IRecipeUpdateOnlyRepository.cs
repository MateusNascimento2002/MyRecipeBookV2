namespace MyRecipeBook.Domain.Interfaces.Repositories.Recipe;

public interface IRecipeUpdateOnlyRepository
{
    Task<Entities.Recipe?> GetById(Guid id, Guid userId);
}