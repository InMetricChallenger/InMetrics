using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Mapster;
using MediatR;
using Throw;

namespace Application.Features.DailySummaries.Queries;
public sealed class GetDailySummaryByDateQueryHandler : IRequestHandler<GetDailySummaryByDateQuery, DailySummaryResponse>
{    
    private readonly IDailySummaryRepository _dailySummaryRepository;

    public GetDailySummaryByDateQueryHandler(IDailySummaryRepository dailySummaryRepository)
    {
        _dailySummaryRepository = dailySummaryRepository;
    }

    public async Task<DailySummaryResponse> Handle(GetDailySummaryByDateQuery request, CancellationToken cancellationToken)
    {
        DailySummary dailySummary = await _dailySummaryRepository.GetDailySummaryByDate(request.Date);
        dailySummary.ThrowIfNull(() => throw new NotFoundException($"Daily summary with date {request.Date.Date} not found"));
        return dailySummary.Adapt<DailySummaryResponse>();
    }
}
