using Application.Features.CashFlows.EventHandlers;
using Domain.Entities;
using Domain.Enums;
using Domain.Events.CashFlowEvents;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InMetrics.Application.Tests.Features.CashFlows.EventHandlers;
public class CashFlowCreatedNotificationHandlerTests
{
    private readonly Mock<IAppQueryDbContext> _queryDbContextMock;
    private readonly TestDatabaseConfiguration _dbConfiguration;
    private readonly DbSet<CashFlow> _cashFlowDbSetMock;

    public CashFlowCreatedNotificationHandlerTests()
    {
        _queryDbContextMock = new Mock<IAppQueryDbContext>();
        _dbConfiguration = new TestDatabaseConfiguration();
        _cashFlowDbSetMock = DbContextMock.GetQueryableMockDbSet<CashFlow>();
        _queryDbContextMock.Setup(q => q.CashFlows).Returns(_cashFlowDbSetMock);
    }

    [Fact]
    public async Task Handle_WhenUsingSingleDatabase_DoesNotAddCashFlowToQueryDbContext()
    {
        
        _dbConfiguration.UseSingleDatabaseValue = true;
        
        var handler = new CashFlowCreatedNotificationHandler(_queryDbContextMock.Object, _dbConfiguration);
        var notification = new CashFlowCreatedEventNotification(1, 10, TransactionType.Debit, DateTime.Now, DateTime.UtcNow);

        
        await handler.Handle(notification, CancellationToken.None);

        
        _queryDbContextMock.Verify(q => q.CashFlows.AddAsync(It.IsAny<CashFlow>(), CancellationToken.None), Times.Never);
        _queryDbContextMock.Verify(q => q.SaveChangesAsync(CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenNotUsingSingleDatabase_AddsCashFlowToQueryDbContextAndSavesChanges()
    {        
        _dbConfiguration.UseSingleDatabaseValue = false;

        var handler = new CashFlowCreatedNotificationHandler(_queryDbContextMock.Object, _dbConfiguration);
        var notification = new CashFlowCreatedEventNotification(1, 10, TransactionType.Debit, DateTime.Now, DateTime.UtcNow);
        
        await handler.Handle(notification, CancellationToken.None);
        
        _queryDbContextMock.Verify(q => q.CashFlows.AddAsync(It.IsAny<CashFlow>(), CancellationToken.None), Times.Once);
        _queryDbContextMock.Verify(q => q.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}
