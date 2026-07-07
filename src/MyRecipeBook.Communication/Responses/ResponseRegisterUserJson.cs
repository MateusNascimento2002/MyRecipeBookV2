namespace MyRecipeBook.Communication.Responses;

public record ResponseRegisterUserJson(Guid Id, string Name, ResponseTokensJson Tokens);