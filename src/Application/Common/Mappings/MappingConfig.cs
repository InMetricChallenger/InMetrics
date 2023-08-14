using Application.Common.Models.DTOs;
using Domain.Entities;
using Mapster;

namespace Application.Common.Mappings;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        if (config is null)
        {
            throw new ArgumentNullException(nameof(config));
        }

        config.NewConfig<CashFlow, CashFlowDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.TransactionType, src => src.TransactionType)
            .Map(dest => dest.Date, src => src.Date);

        config.NewConfig<CashFlowDto, CashFlow>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.TransactionType, src => src.TransactionType)
            .Map(dest => dest.Date, src => src.Date);
    }
}
