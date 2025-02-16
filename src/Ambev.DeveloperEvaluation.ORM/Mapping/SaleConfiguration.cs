using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        // define auto-increment sale number on DB
        builder.Property(u => u.Number).ValueGeneratedOnAdd();
        builder.Property(u => u.CustomerId).IsRequired().HasColumnType("uuid");
        builder.Property(u => u.TotalAmount).IsRequired();
        builder.Property(u => u.BranchId).IsRequired().HasColumnType("uuid");
        builder.Property(u => u.Discount).HasDefaultValue(0);
        builder.Property(u => u.Cancelled).HasDefaultValue(false);
    }
}
