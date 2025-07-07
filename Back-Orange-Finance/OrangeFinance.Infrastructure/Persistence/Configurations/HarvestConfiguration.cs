using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OrangeFinance.Domain.Farms.Models;

namespace OrangeFinance.Infrastructure.Persistence.Configurations;

public sealed class HarvestConfiguration : IEntityTypeConfiguration<HarvestModel>
{
    public void Configure(EntityTypeBuilder<HarvestModel> builder)
    {
        builder.ToTable("Harvests");

        builder.HasKey(m => m.Id);

        builder
            .Property(m => m.Id)
            .ValueGeneratedNever();

        builder.Property(h => h.Description)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(h => h.HarvestDate)
            .IsRequired();

        builder.Property(h => h.Quantity)
            .IsRequired();

        builder.Property(h => h.CropType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(h => h.FarmId)
            .IsRequired();

        builder.HasOne(h => h.Farm)
                .WithMany(f => f.Harvests)
                .HasForeignKey(h => h.FarmId);

    }
}