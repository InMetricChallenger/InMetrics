using Domain.Entities;

namespace InMetrics.Domain.Tests.Entities;
public class DailySummaryTests
{
    [Fact]
    public void DailySummary_AddCredit()
    {
        var dailySummary = new DailySummary(DateTime.UtcNow);
        var creditAmount = 100M;
        dailySummary.AddCredit(creditAmount);
        Assert.Equal(creditAmount, dailySummary.TotalCredit);
    }

    [Fact]
    public void DailySummary_AddDebit()
    {
        var dailySummary = new DailySummary(DateTime.UtcNow);
        var debitAmount = 100M;
        dailySummary.AddDebit(debitAmount);
        Assert.Equal(debitAmount, dailySummary.TotalDebit);
    }

    [Fact]
    public void DailySummary_NetAmount() {
        var dailySummary = new DailySummary(DateTime.UtcNow);
        var debitAmount = 100M;
        dailySummary.AddDebit(debitAmount);

        var creditAmount = 100M;
        dailySummary.AddCredit(creditAmount);
        Assert.Equal(creditAmount - debitAmount, dailySummary.NetAmount);
    }

    [Fact]
    public void DailySummary_EmptyConstructor()
    {
        var dailySummary = new DailySummary();
        Assert.Equal(DateTime.MinValue, dailySummary.Date);
        Assert.Equal(0M, dailySummary.TotalCredit);
        Assert.Equal(0M, dailySummary.TotalDebit);
        Assert.Equal(0M, dailySummary.NetAmount);
    }

    [Fact]
    public void DailySummary_DateConstructor()
    {
        var dateConstructor = DateTime.Now.Date;
        var dailySummary = new DailySummary(dateConstructor);
        Assert.Equal(dateConstructor.Date, dailySummary.Date.Date);
    }
}
