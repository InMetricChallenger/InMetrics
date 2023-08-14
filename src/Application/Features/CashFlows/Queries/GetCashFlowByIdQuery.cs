using Application.Common.Cache;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.CashFlows.Queries;
public sealed class GetCashFlowByIdQuery : CacheableQueryBase, IRequest<CashFlowResponse>, ICacheable
{
    public GetCashFlowByIdQuery(int cashFlowId)
    {
        CashFlowId = cashFlowId;
    }

    public int CashFlowId { get; }

    public override string CacheKey => $"CashFlow:{CashFlowId}";
}