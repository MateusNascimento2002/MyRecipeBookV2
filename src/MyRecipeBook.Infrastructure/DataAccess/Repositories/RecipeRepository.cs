using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
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

    public async Task<IList<RecipeSummaryDto>> GetRecentRecipes(Guid userId)
    {
        return await _context
            .Recipes
            .AsNoTracking()
            .Where(r => r.IsActive && r.UserId == userId)
            .OrderByDescending(r => r.Id)
            .Take(6)
            .Select(r => new RecipeSummaryDto(r.Id, r.Title))
            .ToListAsync();
    }

    public async Task<IList<RecipeSummaryDto>> FilterRecipes(Guid userId, RecipeFilterDto filter)
    {
        var query = _context
            .Recipes
            .AsNoTracking()
            .Where(r => r.IsActive && r.UserId == userId);

        if (filter.CookTime is not null)
            query = query.Where(r => r.CookTime == filter.CookTime.Value);

        if (filter.SearchTerm.IsNotEmpty())
        {
            query = query.Where(r =>
                r.Title.Contains(filter.SearchTerm) ||
                r.Description.Contains(filter.SearchTerm) ||
                r.Ingredients.Any(i => i.Item.Contains(filter.SearchTerm)));
        }

        if (filter.DishTypes.Any())
        {
           var recipesWithDishTypes = query.Where(recipe => recipe.DishTypes.Any(dish => dish.Type == filter.DishTypes[0]));
           
           foreach (var dishType in filter.DishTypes.Skip(1))
           {
               recipesWithDishTypes = recipesWithDishTypes.Union(query.Where(recipe => recipe.DishTypes.Any(dish => dish.Type == dishType)));
           }

           query = recipesWithDishTypes;
        }

        return await query
            .Select(r => new RecipeSummaryDto(r.Id, r.Title))
            .ToListAsync();
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