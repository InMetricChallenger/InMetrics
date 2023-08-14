using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Mapster;
using MediatR;
using Throw;

namespace Application.Features.CashFlows.Queries;
public sealed class GetCashFlowByIdQueryHandler : IRequestHandler<GetCashFlowByIdQuery, CashFlowResponse>
{
    private readonly IBaseRepository<CashFlow> _cashFlowRepository;

    public GetCashFlowByIdQueryHandler(IBaseRepository<CashFlow> cashFlowRepository)
    {
        _cashFlowRepository = cashFlowRepository;
    }

    public async Task<CashFlowResponse> Handle(GetCashFlowByIdQuery request, CancellationToken cancellationToken)
    {
#pragma warning disable CA1062 // Validate arguments of public methods
        CashFlow cashFlow = await _cashFlowRepository.GetByIdAsync(request.CashFlowId).ConfigureAwait(false);
#pragma warning restore CA1062 // Validate arguments of public methods
        cashFlow.ThrowIfNull(() => throw new NotFoundException($"CashFlow with ID {request.CashFlowId} not found"));

        return cashFlow.Adapt<CashFlowResponse>();
    }
}

