using Castle.Core.Resource;
using Domain.Entities;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Application.Features.CashFlows.Commands;
using Domain.Enums;
using System.Linq.Expressions;

namespace InMetrics.Infrastructure.Tests.Repositories;
public class BaseRepositoryTests
{
    private readonly AppCommandDbContext _commandContext;
    private readonly AppQueryDbContext _queryContext;
    private readonly BaseRepository<CashFlow> _repository;

    public BaseRepositoryTests()
    {
        var dbName = Guid.NewGuid().ToString();

        var databaseRoot = new InMemoryDatabaseRoot();

        var options = new DbContextOptionsBuilder<AppCommandDbContext>()
              .UseInMemoryDatabase(dbName, databaseRoot)
              .Options;
        _commandContext = new AppCommandDbContext(options);

        var queryOptions = new DbContextOptionsBuilder<AppQueryDbContext>()
            .UseInMemoryDatabase(dbName, databaseRoot)
            .Options;
        _queryContext = new AppQueryDbContext(queryOptions);

        _repository = new CashFlowRepository(_commandContext, _queryContext);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntityToCommandContext()
    {
        
        var cashFlow = new CashFlow(10, TransactionType.Debit, DateTime.UtcNow);

        
        await _repository.AddAsync(cashFlow);
        await _commandContext.SaveChangesAsync();

        
        var result = await _commandContext.CashFlows.FindAsync(cashFlow.Id);
        Assert.NotNull(result);
        Assert.Equal(cashFlow.Amount, result.Amount);
        Assert.Equal(cashFlow.TransactionType, result.TransactionType);
        Assert.Equal(cashFlow.Date, result.Date);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        var cashFlow1 = new CashFlow(10, TransactionType.Debit, DateTime.UtcNow);
        var cashFlow2 = new CashFlow(15, TransactionType.Credit, DateTime.UtcNow);

        await _repository.AddAsync(cashFlow1);
        await _repository.AddAsync(cashFlow2);

        await _commandContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntityWithGivenId()
    {
        
        var cashFlow = new CashFlow(10, TransactionType.Debit, DateTime.UtcNow);

        await _repository.AddAsync(cashFlow);
        await _commandContext.SaveChangesAsync();
        
        var result = await _repository.GetByIdAsync(cashFlow.Id);
        
        Assert.NotNull(result);
        Assert.Equal(cashFlow.Amount, result.Amount);
        Assert.Equal(cashFlow.TransactionType, result.TransactionType);
        Assert.Equal(cashFlow.Date, result.Date);
    }

    [Fact]
    public async Task Delete_ShouldRemoveEntityFromCommandContext()
    {
        
        var cashFlow = new CashFlow(10, TransactionType.Debit, DateTime.UtcNow);
        await _repository.AddAsync(cashFlow);
        await _commandContext.SaveChangesAsync();
        
        _repository.Delete(cashFlow);
        await _commandContext.SaveChangesAsync();
        
        var result = await _commandContext.CashFlows.FindAsync(cashFlow.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrueIfEntityExists()
    {
        var cashFlow = new CashFlow(10, TransactionType.Debit, DateTime.UtcNow);

        await _repository.AddAsync(cashFlow);
        await _commandContext.SaveChangesAsync();
        
        var exists = await _repository.ExistsAsync(cashFlow.Id);
        
        Assert.True(exists);
    }

    [Fact]
    public async Task CountAsync_ShouldReturnNumberOfEntities()
    {
        
        var cashFlow = new CashFlow(10, TransactionType.Debit, DateTime.UtcNow);
        var cashFlow2 = new CashFlow(10, TransactionType.Debit, DateTime.UtcNow);

        await _repository.AddAsync(cashFlow);
        await _repository.AddAsync(cashFlow2);
        await _commandContext.SaveChangesAsync();

        
        var count = await _repository.CountAsync();
        
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNullForNonExistingEntity()
    {        
        var nonExistingId = -999999999;        
        var result = await _repository.GetByIdAsync(nonExistingId);        
        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalseForNonExistingEntity()
    {
        
        var nonExistingId = 1;        
        var exists = await _repository.ExistsAsync(nonExistingId);        
        Assert.False(exists);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntityInCommandContext()
    {        
        var cashFlow = new CashFlow(10, TransactionType.Debit, DateTime.UtcNow);
        await _repository.AddAsync(cashFlow);
        await _commandContext.SaveChangesAsync();


        cashFlow.Update(15, TransactionType.Credit);
        _repository.Update(cashFlow);
        await _commandContext.SaveChangesAsync();

        // Assert
        var result = await _commandContext.CashFlows.FindAsync(cashFlow.Id);
        Assert.NotNull(result);
        Assert.Equal(15, result.Amount);
        Assert.Equal(TransactionType.Credit, result.TransactionType);
    }

    [Fact]
    public async Task FindAsync_ShouldReturnEntitiesMatchingPredicate()
    {        
        var cashFlow1 = new CashFlow(10, TransactionType.Debit, DateTime.UtcNow);
        var cashFlow2 = new CashFlow(15, TransactionType.Debit, DateTime.UtcNow);
        var cashFlow3 = new CashFlow(10, TransactionType.Credit, DateTime.UtcNow);

        await _repository.AddAsync(cashFlow1);
        await _repository.AddAsync(cashFlow2);
        await _repository.AddAsync(cashFlow3);
        await _commandContext.SaveChangesAsync();

        Expression<Func<CashFlow, bool>> predicate = c => c.TransactionType.Equals(TransactionType.Debit);
        
        var result = await _repository.FindAsync(predicate);        
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnEntitiesInRequestedPage()
    {
        // Arrange
        var cashFlow1 = new CashFlow(10, TransactionType.Debit, DateTime.UtcNow);
        var cashFlow2 = new CashFlow(15, TransactionType.Debit, DateTime.UtcNow);
        var cashFlow3 = new CashFlow(10, TransactionType.Credit, DateTime.UtcNow);
        var cashFlow4 = new CashFlow(20, TransactionType.Credit, DateTime.UtcNow);

        await _repository.AddAsync(cashFlow1);
        await _repository.AddAsync(cashFlow2);
        await _repository.AddAsync(cashFlow3);
        await _repository.AddAsync(cashFlow4);
        await _commandContext.SaveChangesAsync();

        int pageNumber = 2;
        int pageSize = 2;

        // Act
        var result = await _repository.GetPagedAsync(pageNumber, pageSize);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, c => c.Id == cashFlow3.Id);
        Assert.Contains(result, c => c.Id == cashFlow4.Id);
    }
}

