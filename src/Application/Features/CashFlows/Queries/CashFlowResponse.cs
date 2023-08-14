using Domain.Enums;

namespace Application.Features.CashFlows.Queries;
public sealed record CashFlowResponse(int Id, decimal Amount, TransactionType TransactionType, DateTime Date, DateTime Created);