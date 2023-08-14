using Domain.Common;
using Domain.Enums;
using Domain.Events.CashFlowEvents;
using MediatR;
using System.Collections.ObjectModel;

namespace Domain.Entities;

public class CashFlow: BaseAuditableEntity
{
    private readonly List<INotification> _domainEvents = new();

    public IReadOnlyCollection<INotification> DomainEvents => new ReadOnlyCollection<INotification>(_domainEvents);

    public decimal Amount { get; private set; }

    public TransactionType TransactionType { get; private set; }

    public DateTime Date { get; private set; }

    public CashFlow(decimal amount, TransactionType transactionType, DateTime date) 
        : base()
    {
        Amount = amount;
        TransactionType = transactionType;
        Date = date;
    }

    public CashFlow(int id, decimal amount, TransactionType transactionType, DateTime date)
        : base()
    {
        Id = id;
        Amount = amount;
        TransactionType = transactionType;
        Date = date;
    }

    public CashFlow()
    {
    }

    public void AddCashFlowCreatedEvent()
    {
        //if (TransactionType == TransactionType.Credit)
        //{
        //    _domainEvents.Add(new CreditTransactionCreatedEventNotification(Amount, Date, DateTime.UtcNow));
        //}else if (TransactionType == TransactionType.Debit)
        //{
        //    _domainEvents.Add(new DebitTransactionCreatedEventNotification(Amount, Date, DateTime.UtcNow));
        //}
        _domainEvents.Add(new CashFlowCreatedEventNotification(Id, Amount, TransactionType, Date, DateTime.UtcNow));
    }

    public void Update(decimal amount, TransactionType transactionType)
    {
        Amount = amount;
        TransactionType = transactionType;        
    }
}

