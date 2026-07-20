using System.Net;

namespace MyRecipeBook.Exception.ExceptionBase;

public abstract class MyRecipeBookBaseException : System.Exception
{
    public abstract List<string> GetErrorMessages();
    public abstract HttpStatusCode GetStatusCode();
}