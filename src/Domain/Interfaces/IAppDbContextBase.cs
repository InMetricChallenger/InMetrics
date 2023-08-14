using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces;

public interface IAppDbContextBase
{
    DbSet<CashFlow> CashFlows { get; }
    DbSet<DailySummary> DailySummaries { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}