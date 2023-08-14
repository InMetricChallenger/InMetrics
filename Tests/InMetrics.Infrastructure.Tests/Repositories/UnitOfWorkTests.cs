using Castle.Core.Resource;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InMetrics.Infrastructure.Tests.Repositories;
public class UnitOfWorkTests: IDisposable
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppQueryDbContext _queryContext;
    private readonly IAppCommandDbContext _commandContext;

    public UnitOfWorkTests()
    {
        var services = new ServiceCollection();

        // In-memory database setup
        services.AddDbContext<AppQueryDbContext>(options =>
        {
            options.UseInMemoryDatabase("TestDbQuery");
        });
        services.AddDbContext<AppCommandDbContext>(options =>
        {
            options.UseInMemoryDatabase("TestDbCommand");
        });

        services.AddScoped<ICashFlowRepository, CashFlowRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        var serviceProvider = services.BuildServiceProvider();

        _queryContext = serviceProvider.GetRequiredService<AppQueryDbContext>();
        _commandContext = serviceProvider.GetRequiredService<AppCommandDbContext>();
        _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldSaveChangesToDatabase()
    {
        // Arrange
        var cashFlow = new CashFlow(10, TransactionType.Debit, DateTime.UtcNow);
        await _unitOfWork.GetRepository<ICashFlowRepository, CashFlow>().AddAsync(cashFlow);
        await _unitOfWork.SaveChangesAsync();

        // Act
        var result = await _commandContext.CashFlows.FindAsync(cashFlow.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cashFlow.Amount, result.Amount);
        Assert.Equal(cashFlow.TransactionType, result.TransactionType);
        Assert.Equal(cashFlow.Date, result.Date);
    }

    [Fact]
    public void GetRepository_ShouldReturnRepositoryInstance()
    {
        // Act
        var result = _unitOfWork.GetRepository<ICashFlowRepository, CashFlow>();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CashFlowRepository>(result);
    }

    public void Dispose()
    {
        _commandContext.Dispose();
        _queryContext.Dispose();
    }
}
