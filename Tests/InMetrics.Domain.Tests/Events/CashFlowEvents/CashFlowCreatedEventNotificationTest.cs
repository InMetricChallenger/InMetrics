using Domain.Entities;
using Domain.Enums;
using Domain.Events.CashFlowEvents;
using System.Diagnostics.CodeAnalysis;

namespace InMetrics.Domain.Tests.Events.CashFlowEvents;

public class CashFlowCreatedEventNotificationTest
{
    [Trait("CashFlow", "Domain event tests")]
    [Fact(DisplayName = "Cash Flow Created Event Notification - Constructor Sets Values")]    
    public void CashFlowCreatedEventNotification_Constructor_SetsValues()
    {
        int cashFlowId = 1;
        decimal amount = 10.1M;
        TransactionType transactionType = TransactionType.Credit;
        DateTime date = DateTime.Now;
        DateTime eventDateTime = DateTime.UtcNow;

        var eventNotification = new CashFlowCreatedEventNotification(cashFlowId, amount, transactionType, date, eventDateTime);

        Assert.Equal(cashFlowId, eventNotification.CashFlowId);
        Assert.Equal(amount, eventNotification.Amount);
        Assert.Equal(transactionType, eventNotification.TransactionType);
        Assert.Equal(date, eventNotification.Date);
        Assert.Equal(eventDateTime, eventNotification.EventDateTime);
    }
}
