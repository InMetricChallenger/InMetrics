using Application.Common.Models;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Throw;

namespace Application.Features.CashFlows.Commands;
public sealed class CreateCashFlowCommandHandler : IRequestHandler<CreateCashFlowCommand, Result<int>>
{
    private readonly ICashFlowRepository _cashFlowRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public CreateCashFlowCommandHandler(ICashFlowRepository cashFlowRepository, IUnitOfWork unitOfWork, IMediator mediator)
    {
        _cashFlowRepository = cashFlowRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Result<int>> Handle(CreateCashFlowCommand request, CancellationToken cancellationToken)
    {
        request.ThrowIfNull(() => throw new ArgumentNullException(nameof(request)));        
        CashFlow cashFlow = new CashFlow(request.Amount, request.TransactionType, request.Date);
        cashFlow.AddCashFlowCreatedEvent();

        await _cashFlowRepository.AddAsync(cashFlow).ConfigureAwait(false);
        if (cashFlow.DomainEvents != null)
        {
            foreach (var domainEvent in cashFlow.DomainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken).ConfigureAwait(false);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return ResultFactory.Success(cashFlow.Id);
    }
}
