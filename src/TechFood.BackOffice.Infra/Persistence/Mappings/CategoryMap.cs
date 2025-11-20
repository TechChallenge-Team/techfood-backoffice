using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using TechFood.BackOffice.Domain.Entities;

namespace TechFood.Infra.Persistence.Mappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToCollection("categories");

         builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.ImageFileName)
            .HasMaxLength(50)
            .IsRequired();
    }
}
