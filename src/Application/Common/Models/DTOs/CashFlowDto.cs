using Domain.Enums;

namespace Application.Common.Models.DTOs;

public class CashFlowDto
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public TransactionType TransactionType { get; set; }

    public DateTime Date { get; set; }
}
