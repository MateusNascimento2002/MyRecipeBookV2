using MyRecipeBook.Communication.Enums.Recipe;

namespace MyRecipeBook.Communication.Responses;

public class ResponseRecipeJson
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public IList<string> Ingredients { get; set; } = [];
    public IList<ResponseInstructionJson> Instructions { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
    public CookTime CookTime { get; set; }
}