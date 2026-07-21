using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;

namespace MyRecipeBook.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is MyRecipeBookBaseException validationException)
        {
            context.HttpContext.Response.StatusCode = (int)validationException.GetStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(validationException.GetErrorMessages()));
        }
        else
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKOWN_ERROR));
        }
    }
}