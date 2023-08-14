using Domain.Entities;

namespace InMetrics.Application.Tests.Features.CashFlows;
public class CashFlowTests
{
    [Fact]
    public void InternalConstructor_ShouldInitializeProperties()
    {
        
        var cashFlow = new CashFlow();
        
        Assert.NotNull(cashFlow);
        Assert.Equal(0, cashFlow.Id);        
    }
}
