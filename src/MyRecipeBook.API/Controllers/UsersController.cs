using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserAccountUseCase useCase,
        [FromBody] RequestRegisterUserAccountJson request)
    {
        var result = await useCase.Execute(request);
        return Created($"users/{result.Id}", result);
    }
}