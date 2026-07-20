using System.Net;

namespace MyRecipeBook.Exception.ExceptionBase;

public class InvalidLoginException : MyRecipeBookBaseException
{
    public override List<string> GetErrorMessages() => [ResourceMessagesException.VALIDATION_LOGIN_INVALID];
    
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}