using System.Net;

namespace MyRecipeBook.Exception.ExceptionBase;

public class ErrorOnValidationException(List<string> validationErrors) : MyRecipeBookBaseException
{
    public override List<string> GetErrorMessages()
    {
        return validationErrors;
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.BadRequest;
    }
}