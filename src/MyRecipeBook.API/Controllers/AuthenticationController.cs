using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.Login.WithEmailAndPassword;
using MyRecipeBook.Application.UseCases.PasswordRecovery;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] ILoginWithEmailAndPasswordUseCase useCase,
        [FromBody] RequestLoginJson request)
    {
        var result = await useCase.Execute(request);

        return Ok(result);
    }

    [HttpPost("password-recovery")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> PasswordRecovery(
        [FromServices] IRequestPasswordRecoveryCodeUseCase useCase,
        [FromBody] RequestPasswordRecoveryJson request)
    {
        await useCase.Execute(request);

        return Accepted();
    }
}