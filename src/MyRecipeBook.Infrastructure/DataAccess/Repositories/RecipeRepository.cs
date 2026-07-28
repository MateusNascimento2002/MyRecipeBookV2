using Microsoft.EntityFrameworkCore;
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

    public async Task<Recipe?> GetById(Guid id, Guid userId)
    {
        return await _context
            .Recipes
            .Include(r => r.Instructions.OrderBy(i => i.Order))
            .Include(r => r.DishTypes)
            .Include(r => r.Ingredients)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.IsActive && r.Id == id && r.UserId == userId);
    }
}