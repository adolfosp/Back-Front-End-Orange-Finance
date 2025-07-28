using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrangeFinance.Domain.Finances.Models;

namespace OrangeFinance.Infrastructure.Persistence.Configurations;

internal sealed class FinanceConfiguration : IEntityTypeConfiguration<FinanceModel>
{
    public void Configure(EntityTypeBuilder<FinanceModel> builder)
    {
        builder.ToTable("Finances");

        builder.HasKey(f => f.Id);

        builder.Property(h => h.Id).ValueGeneratedOnAdd();

        builder.Property(f => f.Money)
               .IsRequired();

        builder.Property(f => f.Total)
               .IsRequired();

        builder.Property(f => f.Quantity)
               .IsRequired();

        builder.Property(f => f.TypeTransaction)
               .IsRequired()
               .HasConversion<string>();

        builder.HasOne(f => f.Harvest)
               .WithMany(h => h.Finances)
               .HasForeignKey(f => f.HarvestId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}