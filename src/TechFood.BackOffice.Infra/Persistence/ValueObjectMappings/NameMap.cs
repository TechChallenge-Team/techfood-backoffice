using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechFood.BackOffice.Domain.ValueObjects;

namespace TechFood.BackOffice.Infra.Persistence.ValueObjectMappings;

public static class NameMap
{
    public static void MapName<TEntity>(this OwnedNavigationBuilder<TEntity, Name> navigationBuilder)
        where TEntity : class
    {
        navigationBuilder.WithOwner();

        navigationBuilder.Property(x => x.FullName)
            .HasMaxLength(255)
            .HasColumnName("NameFullName")
            .IsRequired();
    }
}
