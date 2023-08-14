using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Application.Features.DailySummaries.EventHandlers;
using Domain.Events.CashFlowEvents;

namespace InMetrics.Application.Tests.Features.DailySummaries.EventHandlers;
public class CreditTransactionCreatedNotificationHandlerTests
{
    private readonly Mock<IAppQueryDbContext> _queryDbContextMock;
    private readonly TestDatabaseConfiguration _dbConfiguration;
    private readonly DbSet<CashFlow> _cashFlowDbSetMock;
    private readonly DbSet<DailySummary> _dailySummaryDbSetMock;
    private readonly Mock<IDailySummaryRepository> _dailySummaryRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public CreditTransactionCreatedNotificationHandlerTests()
    {
        _queryDbContextMock = new Mock<IAppQueryDbContext>();
        _dbConfiguration = new TestDatabaseConfiguration();
        _cashFlowDbSetMock = DbContextMock.GetQueryableMockDbSet<CashFlow>();
        _dailySummaryDbSetMock = DbContextMock.GetQueryableMockDbSet<DailySummary>();
        _queryDbContextMock.Setup(q => q.CashFlows).Returns(_cashFlowDbSetMock);
        _queryDbContextMock.Setup(q => q.DailySummaries).Returns(_dailySummaryDbSetMock);
        _dailySummaryRepositoryMock = new Mock<IDailySummaryRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

    }

    [Fact]
    public async Task Handle_ShouldNotAddCredit_WhenTransactionTypeIsNotCredit()
    {
        var handler = new CreditTransactionCreatedNotificationHandler(_queryDbContextMock.Object, _dbConfiguration, _dailySummaryRepositoryMock.Object, _unitOfWorkMock.Object);
        var notification = new CashFlowCreatedEventNotification(1, 10m, Domain.Enums.TransactionType.Debit, DateTime.UtcNow, DateTime.UtcNow);

        await handler.Handle(notification, CancellationToken.None);

        _dailySummaryRepositoryMock.Verify(x => x.GetDailySummaryByDate(It.IsAny<DateTime>()), Times.Never());
        _dailySummaryRepositoryMock.Verify(x => x.AddAsync(It.IsAny<DailySummary>()), Times.Never());
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Never());
    }

    [Fact]
    public async Task Handle_ShouldAddCredit_WhenTransactionTypeIsCreditAndSummaryExists()
    {
        var existingSummary = new DailySummary(1, DateTime.Today);
        _dailySummaryRepositoryMock.Setup(x => x.GetDailySummaryByDate(It.IsAny<DateTime>())).ReturnsAsync(existingSummary);


        var handler = new CreditTransactionCreatedNotificationHandler(_queryDbContextMock.Object, _dbConfiguration, _dailySummaryRepositoryMock.Object, _unitOfWorkMock.Object);
        var notification = new CashFlowCreatedEventNotification(1, 10m, Domain.Enums.TransactionType.Credit, DateTime.UtcNow, DateTime.UtcNow);

        await handler.Handle(notification, CancellationToken.None);

        Assert.Equal(10m, existingSummary.TotalCredit);
        _dailySummaryRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<DailySummary>()), Times.Never());
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once());
        _queryDbContextMock.Verify(ctx => ctx.DailySummaries.AddAsync(It.IsAny<DailySummary>(), It.IsAny<CancellationToken>()), Times.Never());
        _queryDbContextMock.Verify(ctx => ctx.DailySummaries.Update(existingSummary), Times.Once());
        _queryDbContextMock.Verify(ctx => ctx.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task Handle_ShouldAddCredit_WhenTransactionTypeIsCreditAndSummaryDoesNotExist()
    {
        _dailySummaryRepositoryMock.Setup(repo => repo.GetDailySummaryByDate(It.IsAny<DateTime>())).ReturnsAsync((DailySummary)null);

        var handler = new CreditTransactionCreatedNotificationHandler(_queryDbContextMock.Object, _dbConfiguration, _dailySummaryRepositoryMock.Object, _unitOfWorkMock.Object);
        var notification = new CashFlowCreatedEventNotification(1, 10m, Domain.Enums.TransactionType.Credit, DateTime.UtcNow, DateTime.UtcNow);

        await handler.Handle(notification, CancellationToken.None);
        
        _dailySummaryRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<DailySummary>()), Times.Once());
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once());
        _queryDbContextMock.Verify(ctx => ctx.DailySummaries.AddAsync(It.IsAny<DailySummary>(), It.IsAny<CancellationToken>()), Times.Once());
        _queryDbContextMock.Verify(ctx => ctx.DailySummaries.Update(It.IsAny<DailySummary>()), Times.Never());
        _queryDbContextMock.Verify(ctx => ctx.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}
