using System.ComponentModel.DataAnnotations;

namespace MyRecipeBook.Domain.Entities;

public class RecipeInstruction : EntityBase
{
    public int Order { get; set; }
    [MaxLength(256)]
    public string Description { get; set; } = string.Empty;
    public Guid RecipeId { get; private set; }
}