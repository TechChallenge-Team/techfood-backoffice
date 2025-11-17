using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechFood.BackOffice.Domain.Entities;

namespace TechFood.Infra.Persistence.Mappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Category");

        builder.Property(c => c.Name)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.ImageFileName)
            .HasMaxLength(50)
            .IsRequired();
    }
}
