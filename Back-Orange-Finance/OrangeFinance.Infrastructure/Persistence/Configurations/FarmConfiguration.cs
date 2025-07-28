using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OrangeFinance.Domain.Farms.Models;

namespace OrangeFinance.Infrastructure.Persistence.Configurations;

internal sealed class FarmConfiguration : IEntityTypeConfiguration<FarmModel>
{
    public void Configure(EntityTypeBuilder<FarmModel> builder)
    {
        builder.ToTable("Farms");

        builder.HasKey(m => m.Id);

        builder
           .Property(m => m.Id)
           .ValueGeneratedNever();

        builder
          .Property(m => m.Name)
          .HasMaxLength(100);

        builder
         .Property(m => m.Description)
         .HasMaxLength(300);

        builder
         .Property(m => m.Description)
         .HasMaxLength(300);

        builder
         .Property(m => m.Longitude);

        builder
         .Property(m => m.Latitude);

        builder
         .Property(m => m.Size);

        builder
         .Property(m => m.Type);

        builder
         .Property(m => m.Image)
         .IsRequired(false);
    }
}
