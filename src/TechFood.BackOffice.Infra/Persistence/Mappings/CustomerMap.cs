using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechFood.BackOffice.Domain.Entities;
using TechFood.Infra.Persistence.ValueObjectMappings;

namespace TechFood.Infra.Persistence.Mappings;

public class CustomerMap : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customer");

        builder.OwnsOne(a => a.Name, name => name.MapName())
            .Navigation(a => a.Name)
            .IsRequired();

        builder.OwnsOne(x => x.Email, email => email.MapEmail())
            .Navigation(x => x.Email)
            .IsRequired();

        builder.OwnsOne(x => x.Document, document => document.MapDocument())
            .Navigation(x => x.Document)
            .IsRequired();

        builder.OwnsOne(x => x.Phone, phone => phone!.MapPhone())
            .Navigation(x => x.Phone);
    }
}
