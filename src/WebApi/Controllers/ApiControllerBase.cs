﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using WebApi.Filters;
using WebApi.Presenters;

namespace WebApi.Controllers;
/// <summary>
/// Representa a API base para as outras controllers.
/// </summary>
[ApiController]
[ApiExceptionHandlingFilter]
[Route("api/v{version:apiVersion}/[controller]")]
[ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
[ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status401Unauthorized)]
[Produces("application/json")]
[ExcludeFromCodeCoverage]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender _sender;

    protected ApiControllerBase()
    {
    }

    protected ApiControllerBase(ISender sender)
    {
        _sender = sender;
    }

    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetService<ISender>();
}
