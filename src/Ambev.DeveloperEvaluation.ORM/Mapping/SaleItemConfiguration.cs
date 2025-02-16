using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

internal sealed class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(c => c.Id);
        builder.Property(u => u.ProductId).IsRequired().HasColumnType("uuid");
        builder.Property(u => u.Quantity).IsRequired();
        builder.Property(u => u.Price).IsRequired();
        builder.Property(u => u.TotalAmount).IsRequired();

        builder.HasOne<Sale>()
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.SaleId);
    }
}