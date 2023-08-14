using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events.CashFlowEvents;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.CashFlows.EventHandlers;
public class CashFlowCreatedNotificationHandler : INotificationHandler<CashFlowCreatedEventNotification>
{
    private readonly IAppQueryDbContext _queryDbContext;
    private readonly IDbOperationConfiguration _dbOperationConfiguration;

    public CashFlowCreatedNotificationHandler(IAppQueryDbContext queryDbContext, IDbOperationConfiguration dbOperationConfiguration)
    {
        _queryDbContext = queryDbContext;
        _dbOperationConfiguration = dbOperationConfiguration;
    }

    public async Task Handle(CashFlowCreatedEventNotification notification, CancellationToken cancellationToken)
    {
        if (_dbOperationConfiguration.UseSingleDatabase()) return;

        CashFlow cashFlow = new CashFlow(notification.Amount, notification.TransactionType, notification.Date)
        {
            Created = notification.EventDateTime
        };
        
        //add to read database;
        await _queryDbContext.CashFlows.AddAsync(cashFlow, cancellationToken).ConfigureAwait(false);
        await _queryDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    }
}
