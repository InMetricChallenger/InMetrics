using Application.Common.Cache;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.DailySummaries.Queries;
public sealed class GetDailySummaryByDateQuery : CacheableQueryBase, IRequest<DailySummaryResponse>, ICacheable
{
    public GetDailySummaryByDateQuery(DateTime date)
    {
        Date = date;
    }

    public DateTime Date { get; }

    public override string CacheKey => $"DailySummaryDate:{Date.Date}";
}
