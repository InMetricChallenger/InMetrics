using Application.Common.Interfaces;
using Application.Features.DailySummaries.Queries;
using Application.Settings;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Moq;

namespace InMetrics.Application.Tests.Features.DailySummaries.Queries;
public class GetDailySummaryByDateQueryHandlerTests
{
    
    private readonly Mock<IDailySummaryRepository> _dailySummaryRepositoryMock;
    private readonly GetDailySummaryByDateQueryHandler _handler;
    private readonly ICustomServiceProvider _serviceProvider;
    private readonly CacheSettings _cacheSettings;

    public GetDailySummaryByDateQueryHandlerTests()
    {
        _dailySummaryRepositoryMock = new Mock<IDailySummaryRepository>();
        _handler = new GetDailySummaryByDateQueryHandler(_dailySummaryRepositoryMock.Object);
        

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

        DateTime date = DateTime.UtcNow.Date;
        var query = new GetDailySummaryByDateQuery(date);

        string cacheKey = query.CacheKey;

        Assert.Equal($"DailySummaryDate:{date}", cacheKey);
    }

    [Fact]
    public void GetCacheExpiration_ShouldReturnCorrectValueFromCacheSettings()
    {
        
        DateTime date = DateTime.UtcNow.Date;
        var query = new GetDailySummaryByDateQuery(date);
        
        TimeSpan? cacheExpiration = query.GetCacheExpiration(_serviceProvider);
        
        Assert.Equal(TimeSpan.FromMinutes(_cacheSettings.ExpirationInMinutes), cacheExpiration);
    }

    [Fact]
    public async Task Handle_CashFlowFound_ShouldReturnCashFlowResponse()
    {

        DateTime dailySummaryDate = DateTime.Now.Date;
        var dailySummary = CreateDailySummaryWithDate(dailySummaryDate.Date);
        _dailySummaryRepositoryMock            
            .Setup(repo => repo.GetDailySummaryByDate(dailySummaryDate))
            .ReturnsAsync(dailySummary);

        var query = new GetDailySummaryByDateQuery(dailySummaryDate);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dailySummaryDate, result.Date);        
    }

    private static DailySummary CreateDailySummaryWithDate(DateTime date)
    {
        DailySummary dailySummary = new DailySummary(date);        
        typeof(DailySummary).GetProperty("Date")?.SetValue(dailySummary, date);
        return dailySummary;
    }
}
