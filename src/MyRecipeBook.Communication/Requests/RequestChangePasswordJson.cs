namespace MyRecipeBook.Communication.Requests;

public record RequestChangePasswordJson(string CurrentPassword, string NewPassword);