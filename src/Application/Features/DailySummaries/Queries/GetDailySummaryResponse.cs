namespace Application.Features.DailySummaries.Queries;
public sealed record DailySummaryResponse(int Id, DateTime Date, decimal TotalCredit, decimal TotalDebit, decimal NetAmount, DateTime Created);
