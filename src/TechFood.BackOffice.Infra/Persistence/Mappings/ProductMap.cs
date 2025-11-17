using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechFood.BackOffice.Domain.Entities;

namespace TechFood.Infra.Persistence.Mappings;

public class ProductMap : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");

        builder.Property(a => a.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(a => a.Description)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasOne<Category>()
            .WithMany()
            .IsRequired();

        builder.Property(a => a.ImageFileName)
            .HasMaxLength(50)
            .IsRequired();
    }
}
