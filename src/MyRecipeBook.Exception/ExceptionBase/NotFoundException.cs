using System.Net;

namespace MyRecipeBook.Exception.ExceptionBase;

public class NotFoundException: MyRecipeBookBaseException
{
    private readonly string _message;

    public NotFoundException(string message)
    {
        _message = message;
    }
    
    public override List<string> GetErrorMessages()
    {
        return [_message];
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.NotFound;
    }
}