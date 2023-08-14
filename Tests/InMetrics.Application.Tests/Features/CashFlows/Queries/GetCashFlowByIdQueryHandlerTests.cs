using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Features.CashFlows.Queries;
using Application.Settings;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Moq;

namespace InMetrics.Application.Tests.Features.CashFlows.Queries;
public class GetCashFlowByIdQueryHandlerTests
{
    private readonly Mock<IBaseRepository<CashFlow>> _cashFlowRepositoryMock;
    private readonly GetCashFlowByIdQueryHandler _handler;
    private readonly ICustomServiceProvider _serviceProvider;
    private readonly CacheSettings _cacheSettings;

    public GetCashFlowByIdQueryHandlerTests()
    {
        _cashFlowRepositoryMock = new Mock<IBaseRepository<CashFlow>>();
        _handler = new GetCashFlowByIdQueryHandler(_cashFlowRepositoryMock.Object);

        var serviceProviderMock = new Mock<ICustomServiceProvider>();

        _cacheSettings = new CacheSettings
        {
            ExpirationInMinutes = 5
        };

        serviceProviderMock
            .Setup(x => x.GetService<IOptions<CacheSettings>>())
            .Returns(Options.Create(_cacheSettings));

        _serviceProvider = serviceProviderMock.Object;
    }

    [Fact]
    public void CacheKey_ShouldReturnCorrectValue()
    {
        
        int cashFlowId = 1;
        var query = new GetCashFlowByIdQuery(cashFlowId);
        
        string cacheKey = query.CacheKey;
        
        Assert.Equal($"CashFlow:{cashFlowId}", cacheKey);
    }

    [Fact]
    public void GetCacheExpiration_ShouldReturnCorrectValueFromCacheSettings()
    {
        // Arrange
        int cashFlowId = 1;
        var query = new GetCashFlowByIdQuery(cashFlowId);

        // Act
        TimeSpan? cacheExpiration = query.GetCacheExpiration(_serviceProvider);

        // Assert
        Assert.Equal(TimeSpan.FromMinutes(_cacheSettings.ExpirationInMinutes), cacheExpiration);
    }

    [Fact]
    public async Task Handle_CashFlowFound_ShouldReturnCashFlowResponse()
    {
        // Arrange
        int cashFlowId = 1;
        var cashFlow = CreateCashFlowWithId(cashFlowId, 10, TransactionType.Credit, DateTime.Now);
        _cashFlowRepositoryMock
            .Setup(repo => repo.GetByIdAsync(cashFlowId))
            .ReturnsAsync(cashFlow);

        var query = new GetCashFlowByIdQuery(cashFlowId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cashFlowId, result.Id);
        Assert.Equal(cashFlow.Amount, result.Amount);
        Assert.Equal(cashFlow.TransactionType, result.TransactionType);
        Assert.Equal(cashFlow.Date, result.Date);
    }

    [Fact]
    public async Task Handle_CashFlowNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        int cashFlowId = 1;
        _cashFlowRepositoryMock
            .Setup(repo => repo.GetByIdAsync(cashFlowId))
            .ReturnsAsync((CashFlow)null);

        var query = new GetCashFlowByIdQuery(cashFlowId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }

    private static CashFlow CreateCashFlowWithId(int id, decimal amount, TransactionType transactionType, DateTime date)
    {
        CashFlow cashFlow = new CashFlow(id, amount, transactionType, date);        
        typeof(CashFlow).GetProperty("Id")?.SetValue(cashFlow, id);
        return cashFlow;
    }
}