using MyRecipeBook.Domain.Dtos;

namespace MyRecipeBook.Domain.Interfaces.Repositories.Recipe;

public interface IRecipeReadOnlyRepository
{
    Task<Entities.Recipe?> GetById(Guid id, Guid userId);
    Task<IList<RecipeSummaryDto>> GetRecentRecipes(Guid userId);
    Task<IList<RecipeSummaryDto>> FilterRecipes(Guid userId, RecipeFilterDto filter);
}