using Application.Common.Mappings;
using Application.Common.Models.DTOs;
using Domain.Entities;
using Domain.Enums;
using Mapster;

namespace InMetrics.Application.Tests.Mappings;
public class MappingConfigTests
{
    private readonly TypeAdapterConfig _typeAdapterConfig;

    public MappingConfigTests()
    {
        _typeAdapterConfig = new TypeAdapterConfig();
        new MappingConfig().Register(_typeAdapterConfig);
    }

    [Fact]
    public void CashFlowToCashFlowDtoMappingConfiguration_ShouldBeValid()
    {
        var cashFlow = new CashFlow(10, TransactionType.Debit, new DateTime(2023, 08, 06));

        var cashFlowDto = cashFlow.Adapt<CashFlowDto>(_typeAdapterConfig);
        Assert.Equal(cashFlow.Id, cashFlowDto.Id);
        Assert.Equal(cashFlow.Amount, cashFlowDto.Amount);
        Assert.Equal(cashFlow.TransactionType, cashFlowDto.TransactionType);
        Assert.Equal(cashFlow.Date, cashFlowDto.Date);
    }

    [Fact]
    public void CashFlowDtoToCashFlowMappingConfiguration_ShouldBeValid()
    {
        var cashFlowDto = new CashFlowDto { Id = 1, Amount = 10, TransactionType =  TransactionType.Debit, Date = new DateTime(2023, 08, 06)};
        var cashFlow = cashFlowDto.Adapt<CashFlow>(_typeAdapterConfig);

        Assert.Equal(cashFlowDto.Id, cashFlow.Id);
        Assert.Equal(cashFlowDto.Amount, cashFlow.Amount);
        Assert.Equal(cashFlowDto.TransactionType, cashFlow.TransactionType);
        Assert.Equal(cashFlowDto.Date, cashFlow.Date);
    }
    [Fact]
    public void MappingConfig_Register_ThrowsArgumentNullException_WhenConfigIsNull()
    {
        // Arrange
        var mappingConfig = new MappingConfig();
        TypeAdapterConfig config = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => mappingConfig.Register(config));
    }
}
