using MyRecipeBook.Communication.Enums.Recipe;

namespace MyRecipeBook.Communication.Requests;

public class RequestRecipeJson
{
    public string Title { get; set; } = string.Empty;
    public CookTime CookTime { get; set; }
    public string Description { get; set; } = string.Empty;
    public IList<string> Ingredients { get; set; } = [];
    public IList<RequestInstructionJson> Instructions { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
}
