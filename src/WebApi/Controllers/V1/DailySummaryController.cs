using Application.Features.DailySummaries.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Presenters;

namespace WebApi.Controllers.V1;
[Route("api/[controller]")]
[ApiController]
public class DailySummaryController : ApiControllerBase
{
    /// <summary>
    /// Obter o resumo do dia pela data fornecida.
    /// </summary>
    /// <param name="date">A data a ser fornecida</param>
    /// <param name="cancellationToken"></param>
    /// <returns>O resumo do dia pela data especificada</returns>
    [HttpGet("{date:datetime}")]
    [ProducesResponseType(typeof(DailySummaryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<IActionResult> GetByDate(DateTime date, CancellationToken cancellationToken)
    {        
        DailySummaryResponse dailySummary = await Sender.Send(new GetDailySummaryByDateQuery(date), cancellationToken).ConfigureAwait(false);

        return Ok(dailySummary);
    }
}
