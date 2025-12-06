using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using TechFood.BackOffice.Domain.Entities;

namespace TechFood.BackOffice.Infra.Persistence.Mappings;

public class CustomerMap : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
       builder.ToCollection("customers");
        
        // Configure primary key
        builder.HasKey(c => c.Id);
        
        // Configure complex properties (Value Objects)
        builder.OwnsOne(c => c.Name, name =>
        {
            name.Property(n => n.FullName);
        });
        
        builder.OwnsOne(c => c.Email, email =>
        {
            email.Property(e => e.Address);
        });
        
        builder.OwnsOne(c => c.Document, document =>
        {
            document.Property(d => d.Type);
            document.Property(d => d.Value);
        });
        
        builder.OwnsOne(c => c.Phone, phone =>
        {
            phone.Property(p => p.Number);
            phone.Property(p => p.CountryCode);
            phone.Property(p => p.DDD);
        });
    }
}
