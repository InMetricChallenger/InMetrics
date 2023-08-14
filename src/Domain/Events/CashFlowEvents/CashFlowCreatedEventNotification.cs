using Domain.Enums;
using MediatR;

namespace Domain.Events.CashFlowEvents;

public class CashFlowCreatedEventNotification: INotification
{

    public int CashFlowId { get; }
    public decimal Amount { get; }
    public TransactionType TransactionType { get; }
    public DateTime Date { get; }

    public DateTime EventDateTime { get; }

    public CashFlowCreatedEventNotification(int cashFlowId, decimal amount, TransactionType transactionType, DateTime date, DateTime eventDateTime)
    {
        CashFlowId = cashFlowId;
        Amount = amount;
        TransactionType = transactionType;
        Date = date;
        EventDateTime = eventDateTime;
    }
}
