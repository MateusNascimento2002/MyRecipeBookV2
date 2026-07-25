using System.ComponentModel.DataAnnotations;
using MyRecipeBook.Domain.Enums.Recipe;

namespace MyRecipeBook.Domain.Entities;

public class Recipe : EntityBase
{
    [MaxLength(256)]
    public string Title { get; set; } = string.Empty;
    public CookTime CookTime { get; set; }
    [MaxLength(512)]
    public string Description { get; set; } = string.Empty;
    public ICollection<RecipeIngredient> Ingredients { get; set; } = [];
    public ICollection<RecipeInstruction> Instructions { get; set; } = [];
    public ICollection<RecipeDishType> DishTypes { get; set; } = [];
    public Guid UserId { get; set; }
}