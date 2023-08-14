using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Persistance;

public abstract class AppDbContextBase : DbContext, IAppDbContextBase
{
    protected AppDbContextBase(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<CashFlow> CashFlows => Set<CashFlow>();
    public DbSet<DailySummary> DailySummaries => Set<DailySummary>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder?.ApplyConfigurationsFromAssembly(typeof(AppDbContextBase).Assembly);
    }
}
