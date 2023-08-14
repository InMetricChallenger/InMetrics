using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Infrastructure.Configurations;
public class DailySummaryConfiguration : IEntityTypeConfiguration<DailySummary>
{
    public void Configure(EntityTypeBuilder<DailySummary> builder)
    {
        builder?.HasKey(x => x.Id);
        builder?.HasIndex(x => x.Date)
            .IsUnique(true);
    }
}
