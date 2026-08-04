using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Application.UseCases.Recipe.GetById;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RecipesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredRecipeJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromServices] IRegisterRecipeUseCase  useCase, [FromBody] RequestRecipeJson request)
    {
        var result = await  useCase.Execute(request);
        return Created(string.Empty, result);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status200OK)]
    [ProducesResponseType( typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromServices] IGetRecipeByIdUseCase useCase, [FromRoute] Guid id)
    {
        var result = await useCase.Execute(id);
        return Ok(result);
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType( typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteById([FromServices] IDeleteRecipeByIdUseCase useCase, [FromRoute] Guid id)
    {
        await useCase.Execute(id);
        return NoContent();
    }
}