using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
public class CashFlowConfiguration : IEntityTypeConfiguration<CashFlow>
{
    public void Configure(EntityTypeBuilder<CashFlow> builder)
    {
        builder?.HasKey(c => c.Id);
        builder?.Property(c => c.Amount).IsRequired();
        builder?.Property(c => c.TransactionType).IsRequired();
        builder?.Property(c => c.Date).IsRequired();
    }
}
