namespace MyRecipeBook.Exception.ExceptionBase;

public class ErrorOnValidationException : MyRecipeBookBaseException
{
    private readonly List<string> _validationErrors;

    public ErrorOnValidationException(List<string> validationErrors)
    {
        _validationErrors = validationErrors;
    }
}