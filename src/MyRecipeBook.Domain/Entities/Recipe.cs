using MyRecipeBook.Domain.Enums.Recipe;

namespace MyRecipeBook.Domain.Entities;

public class Recipe : EntityBase
{
    public string Title { get; set; } = string.Empty;
    public CookTime CookTime { get; set; }
    public string Description { get; set; } = string.Empty;
    public ICollection<RecipeIngredient> Ingredients { get; set; } = [];
    public ICollection<RecipeInstruction> Instructions { get; set; } = [];
    public ICollection<RecipeDishType> DishTypes { get; set; } = [];
    public Guid UserId { get; set; }
}