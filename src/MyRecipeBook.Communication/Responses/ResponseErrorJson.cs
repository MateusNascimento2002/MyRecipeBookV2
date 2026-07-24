namespace MyRecipeBook.Communication.Responses;

public class ResponseErrorJson
{
    public ResponseErrorJson(List<string> errorMessages)
    {
        Errors = errorMessages;
    }

    public ResponseErrorJson(string errorMessage)
    {
        Errors = [errorMessage];
    }

    public ResponseErrorJson(string errorMessage, bool accessTokenExpired)
    {
        Errors = [errorMessage];
        AccessTokenExpired = accessTokenExpired;
    }

    public List<string> Errors { get; private set; }
    public bool AccessTokenExpired { get; private set; }
}