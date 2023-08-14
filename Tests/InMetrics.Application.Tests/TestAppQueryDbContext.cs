using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InMetrics.Application.Tests;
public class TestAppQueryDbContext : DbContext, IAppQueryDbContext
{
    public TestAppQueryDbContext(DbContextOptions<TestAppQueryDbContext> options)
        : base(options)
    {
    }

    public DbSet<CashFlow> CashFlows { get; set; }

    public DbSet<DailySummary> DailySummaries { get; set; }
}
