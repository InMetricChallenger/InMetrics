using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class DailySummaryRepository : BaseRepository<DailySummary>, IDailySummaryRepository
{
    private readonly AppCommandDbContext _commandContext;
    private readonly AppQueryDbContext _queryContext;

    public DailySummaryRepository(AppCommandDbContext commandContext, AppQueryDbContext queryContext) : base(commandContext, queryContext)
    {
        _commandContext = commandContext;
        _queryContext = queryContext;
    }

    public async Task<DailySummary> GetDailySummaryByDate(DateTime date)
    {
        return await _queryContext.DailySummaries.FirstOrDefaultAsync(x => x.Date.Date.Equals(date.Date)).ConfigureAwait(false);
    }
}
