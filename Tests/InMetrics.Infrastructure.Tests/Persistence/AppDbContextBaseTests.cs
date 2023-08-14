using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace InMetrics.Infrastructure.Tests.Persistence;
public class AppDbContextBaseTests
{
    [Fact]
    public void AppDbContextBase_ShouldCreateCashFlowTable()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContextBase>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        // Act
        using (var context = new TestDbContext(options))
        {
            context.Database.EnsureCreated();
            var exists = context.Model.FindEntityType(typeof(CashFlow)) != null;

            // Assert
            Assert.True(exists);
        }
    }

    private class TestDbContext : AppDbContextBase
    {
        public TestDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}

