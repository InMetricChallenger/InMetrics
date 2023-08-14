using Application.Common.Models;
using Application.Features.CashFlows.Commands;
using Domain.Enums;
using FluentValidation.TestHelper;
using OneOf.Types;

namespace InMetrics.Application.Tests.Features.CashFlows.Commands;
public class CreateCashFlowCommandValidatorTests
{
    private readonly CreateCashFlowCommandValidator _validator;

    public CreateCashFlowCommandValidatorTests()
    {
        _validator = new CreateCashFlowCommandValidator();
    }

    //[Fact]
    //public void Validate_InvalidAmount_ShouldHaveValidationError()
    //{
    //    var result = _validator.TestValidate(new CreateCashFlowCommand(0, TransactionType.Debit, DateTime.Now));
    //    result.ShouldNotHaveValidationErrorFor(x => x.Amount);
    //}



    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task Validate_InvalidAmount_ShouldHaveValidationError(decimal amount)
    {
        var result = _validator.TestValidate(new CreateCashFlowCommand(amount, TransactionType.Debit, DateTime.Now));
        result.ShouldHaveValidationErrorFor(x => x.Amount);
        Assert.False(result.IsValid);
    }


    [Trait("CashFlow", "Domain entities tests")]
    [Theory(DisplayName = "Fail to register CashFlow when date is older than 7 days")]
    [InlineData(-1, true)]
    [InlineData(-6, true)]
    [InlineData(0, true)]
    [InlineData(-7, false)]
    [InlineData(-8, false)]
    public void Validate_InvalidDate_ShouldHaveValidationError(int days, bool isValid)
    {
        var result = _validator.TestValidate(new CreateCashFlowCommand(100, TransactionType.Debit, DateTime.UtcNow.AddDays(days)));
        if (isValid)
            result.ShouldNotHaveValidationErrorFor(x => x.Date);
        else        
            result.ShouldHaveValidationErrorFor(x => x.Date);        
        
        Assert.Equal(result.IsValid, isValid);
    }

    [Trait("CashFlow", "Domain entities tests")]
    [Theory(DisplayName = "Fail to register CashFlow when date in future")]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(7, false)]
    [InlineData(8, false)]
    public void FailToRegisterCashFlowWhenDateInFuture(int days, bool isValid)
    {
        var result = _validator.TestValidate(new CreateCashFlowCommand(100, TransactionType.Debit, DateTime.UtcNow.AddDays(days)));
        var cashFlow = new CreateCashFlowCommand(100, TransactionType.Debit, DateTime.Now.AddDays(days));
        if (isValid)
            result.ShouldNotHaveValidationErrorFor(x => x.Date);
        else
            result.ShouldHaveValidationErrorFor(x => x.Date);

        Assert.Equal(result.IsValid, isValid);
    }
}
