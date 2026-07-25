using System.ComponentModel.DataAnnotations;

namespace MyRecipeBook.Domain.Entities;

public class RecipeIngredient : EntityBase
{
    [MaxLength(256)]
    public string Item { get; set; } = string.Empty;
    public Guid RecipeId { get; private set; }
}