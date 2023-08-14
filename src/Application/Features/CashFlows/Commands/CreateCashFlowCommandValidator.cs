using FluentValidation;

namespace Application.Features.CashFlows.Commands;
public sealed class CreateCashFlowCommandValidator : AbstractValidator<CreateCashFlowCommand>
{
    public CreateCashFlowCommandValidator()
    {
        RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Transaction need to be greater than 0.");

        RuleFor(x => x.Date)
                .Must(OlderThanServenDays)
                .WithMessage("Transaction date must be recent.");

        RuleFor(x => x.Date)
                    .Must(TransactionCannotBeInFuture)
                    .WithMessage("Transaction cannot be in future.");

    }

    protected static bool OlderThanServenDays(DateTime transactionDate)
    {
        return transactionDate >= DateTime.UtcNow.AddDays(-7);
    }

    protected static bool TransactionCannotBeInFuture(DateTime transactionDate)
    {
        return transactionDate < DateTime.UtcNow;
    }
}