using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using TechFood.BackOffice.Domain.Entities;
using TechFood.Infra.Persistence.ValueObjectMappings;

namespace TechFood.Infra.Persistence.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToCollection("users");

         builder.HasKey(u => u.Id);

        builder.OwnsOne(u => u.Name, name => name.MapName())
            .Navigation(u => u.Name)
            .IsRequired();

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(255);

        builder.OwnsOne(u => u.Email, email => email.MapEmail())
            .Navigation(u => u.Email);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Role)
            .IsRequired();
    }
}
