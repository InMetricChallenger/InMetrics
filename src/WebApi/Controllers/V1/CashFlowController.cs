using Application.Common.Models;
using Application.Features.CashFlows.Commands;
using Application.Features.CashFlows.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Presenters;

namespace WebApi.Controllers.V1;
[Route("api/[controller]")]
[ApiController]
public class CashFlowController : ApiControllerBase
{
    public CashFlowController(ISender sender)
        : base(sender)
    {
    }

    /// <summary>
    /// Obter um cliente pelo seu ID.
    /// </summary>
    /// <param name="cashFlowId">O ID do cliente a ser obtido.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>O cliente com o ID especificado.</returns>
    [HttpGet("{cashFlowId:int}")]
    [ProducesResponseType(typeof(CashFlowResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status404NotFound)]    
    public async Task<IActionResult> GetById(int cashFlowId, CancellationToken cancellationToken)
    {
        CashFlowResponse cashFlow = await Sender.Send(new GetCashFlowByIdQuery(cashFlowId), cancellationToken).ConfigureAwait(false);

        return Ok(cashFlow);
    }

    /// <summary>
    /// Cadastrar um novo controle de caixa.
    /// </summary>
    /// <param name="command">Dados do controle de caixa a ser criado.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>O ID do controle de caixa criado.</returns>    
    [HttpPost]
    [ProducesResponseType(typeof(Result<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateCashFlowCommand command, CancellationToken cancellationToken)
    {
        Result<int> result = await Sender.Send(command, cancellationToken).ConfigureAwait(false);

        return result.Succeeded
            ? CreatedAtAction(nameof(Create), new { cashFlowId = result.Data }, result)
            : UnprocessableEntity(new CustomProblemDetails(result.Errors));
    }
}
