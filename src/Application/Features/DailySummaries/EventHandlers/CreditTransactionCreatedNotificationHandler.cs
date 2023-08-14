using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Events.CashFlowEvents;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.DailySummaries.EventHandlers;
public class CreditTransactionCreatedNotificationHandler : INotificationHandler<CashFlowCreatedEventNotification>
{
    private readonly IAppQueryDbContext _queryDbContext;
    private readonly IDbOperationConfiguration _dbOperationConfiguration;
    private readonly IDailySummaryRepository _summaryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreditTransactionCreatedNotificationHandler(IAppQueryDbContext queryDbContext, IDbOperationConfiguration dbOperationConfiguration, IDailySummaryRepository summaryRepository, IUnitOfWork unitOfWork)
    {
        _queryDbContext = queryDbContext;
        _dbOperationConfiguration = dbOperationConfiguration;
        _summaryRepository = summaryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CashFlowCreatedEventNotification notification, CancellationToken cancellationToken)
    {
        if (notification.TransactionType != TransactionType.Credit) return;
        var date = notification.Date.Date;
        var dailySummary = await _summaryRepository.GetDailySummaryByDate(date);
        bool summaryExists = true;
        if (dailySummary == null)
        {
            summaryExists = false;
            dailySummary = new DailySummary(date);
            await _summaryRepository.AddAsync(dailySummary).ConfigureAwait(false);
        }
        dailySummary.AddCredit(notification.Amount);
        await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);


        if (_dbOperationConfiguration.UseSingleDatabase()) return;
        if (!summaryExists)
            await _queryDbContext.DailySummaries.AddAsync(dailySummary, cancellationToken).ConfigureAwait(false);
        else
            _queryDbContext.DailySummaries.Update(dailySummary);

        await _queryDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    }
}
