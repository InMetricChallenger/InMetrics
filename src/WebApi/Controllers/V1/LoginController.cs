using Application.Common.Models.DTOs;
using Application.Common.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Presenters;

namespace WebApi.Controllers.V1;
[ApiController]
public class LoginController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(AuthenticationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public IActionResult Index()
    {
        var authDto = new TokenService().GenerateToken();
        return Ok(authDto);
    }
}
