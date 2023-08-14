using Domain.Entities;
using Domain.Enums;
using Domain.Events.CashFlowEvents;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace InMetrics.Domain.Tests.Entities;

[ExcludeFromCodeCoverage]
public class CashFlowTests
{
    [Trait("CashFlow", "Domain entities tests")]
    [Fact]
    public void AddCashFlowCreatedEvent_AddsEventToDomainEvents()
    {
        var cashFlow = new CashFlow(10, TransactionType.Debit, DateTime.Now);
        cashFlow.AddCashFlowCreatedEvent();
        Assert.Single(cashFlow.DomainEvents);
        Assert.IsType<CashFlowCreatedEventNotification>(cashFlow.DomainEvents.First());
    }

    [Fact]
    public void CashFlow_Update_UpdatesProperties()
    {
        
        var cashFlow = new CashFlow(10, TransactionType.Debit, DateTime.UtcNow);
        decimal updateAmount = 15.3M;
        TransactionType updateTransactionType = TransactionType.Credit;

        cashFlow.Update(updateAmount, updateTransactionType);

        
        Assert.Equal(updateAmount, cashFlow.Amount);
        Assert.Equal(updateTransactionType, cashFlow.TransactionType);
    }
}
