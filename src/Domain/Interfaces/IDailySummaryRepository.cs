using Domain.Entities;

namespace Domain.Interfaces;
public interface IDailySummaryRepository: IBaseRepository<DailySummary>
{
    Task<DailySummary> GetDailySummaryByDate(DateTime date);
}
