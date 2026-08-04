using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Interfaces.Repositories.Recipe;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

internal sealed class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository,
    IRecipeUpdateOnlyRepository
{
    private readonly MyRecipeBookDbContext _context;

    public RecipeRepository(MyRecipeBookDbContext context)
    {
        _context = context;
    }

    public async Task Add(Recipe recipe)
    {
        await _context.Recipes.AddAsync(recipe);
    }

    public async Task<bool> DeleteById(Guid id, Guid userId)
    {
        var deletedRows = await _context
            .Recipes
            .Where(r => r.IsActive && r.Id == id && r.UserId == userId)
            .ExecuteDeleteAsync();

        return deletedRows > 0;
    }

    async Task<Recipe?> IRecipeReadOnlyRepository.GetById(Guid id, Guid userId)
    {
        return await GetFullRecipe()
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.IsActive && r.Id == id && r.UserId == userId);
    }

    async Task<Recipe?> IRecipeUpdateOnlyRepository.GetById(Guid id, Guid userId)
    {
        return await GetFullRecipe()
            .FirstOrDefaultAsync(r => r.IsActive && r.Id == id && r.UserId == userId);
    }

    private IIncludableQueryable<Recipe, IOrderedEnumerable<RecipeInstruction>> GetFullRecipe()
    {
        return _context
            .Recipes
            .Include(r => r.DishTypes)
            .Include(r => r.Ingredients)
            .Include(r => r.Instructions.OrderBy(i => i.Order));
    }
}