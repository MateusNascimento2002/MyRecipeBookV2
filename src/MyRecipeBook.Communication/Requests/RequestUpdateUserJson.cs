namespace MyRecipeBook.Communication.Requests;

public record RequestUpdateUserJson()
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}