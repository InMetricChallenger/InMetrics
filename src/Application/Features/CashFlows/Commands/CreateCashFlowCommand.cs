using Application.Common.Models;
using Domain.Enums;
using MediatR;

namespace Application.Features.CashFlows.Commands;
public sealed record CreateCashFlowCommand(decimal Amount, TransactionType TransactionType, DateTime Date) : IRequest<Result<int>>;