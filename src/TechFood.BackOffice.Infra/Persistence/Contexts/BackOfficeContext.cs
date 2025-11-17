using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechFood.Shared.Domain.Entities;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Enums;
using TechFood.Shared.Infra.Persistence.Contexts;

namespace TechFood.Infra.Persistence.Contexts;

public class BackOfficeContext(DbContextOptions<BackOfficeContext> options) : TechFoodContext(options)
{
    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<Customer> Customers { get; set; } = null!;

    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker
            .Entries<Entity>()
            .Where(e => e.State == EntityState.Deleted))
        {
            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BackOfficeContext).Assembly);

        var properties = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetProperties());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(Entity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(Entity.IsDeleted));
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);

                modelBuilder.Entity(entityType.ClrType)
                    .HasKey(nameof(Entity.Id));

                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(Entity.Id))
                    .IsRequired()
                    .ValueGeneratedNever();
            }
        }

        var stringProperties = properties.Where(p => p.ClrType == typeof(string));
        foreach (var property in stringProperties)
        {
            var maxLength = property.GetMaxLength() ?? 50;

            property.SetColumnType($"varchar({maxLength})");
        }

        var booleanProperties = properties
            .Where(p => p.ClrType == typeof(bool) ||
                        p.ClrType == typeof(bool?));

        foreach (var property in booleanProperties)
        {
            property.SetColumnType("bit");
            property.IsNullable = false;
        }

        var dateTimeProperties = properties.Where(p => p.ClrType == typeof(DateTime));

        foreach (var property in dateTimeProperties)
        {
            property.SetColumnType("datetime");
        }

        var enumProperties = properties.Where(p => p.ClrType == typeof(Enum));

        foreach (var property in enumProperties)
        {
            property.SetColumnType("smallint");
        }

        var amountProperties = properties
            .Where(p => p.ClrType == typeof(decimal) ||
                        p.ClrType == typeof(decimal?));

        foreach (var property in amountProperties)
        {
            property.SetColumnType("decimal(6, 2)");
        }

        SeedContext(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void SeedContext(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>()
            .HasData(
                new { Id = new Guid("25b58f54-63bc-42da-8cf6-8162097e72c8"), IsDeleted = false }
            );

        modelBuilder.Entity<Customer>().OwnsOne(c => c.Name)
           .HasData(
               new { CustomerId = new Guid("25b58f54-63bc-42da-8cf6-8162097e72c8"), FullName = "John" }
           );

        modelBuilder.Entity<Customer>().OwnsOne(c => c.Document)
          .HasData(
              new { CustomerId = new Guid("25b58f54-63bc-42da-8cf6-8162097e72c8"), Type = DocumentType.CPF, Value = "63585272070" }
          );

        modelBuilder.Entity<Customer>().OwnsOne(c => c.Email)
          .HasData(
              new { CustomerId = new Guid("25b58f54-63bc-42da-8cf6-8162097e72c8"), Address = "john.dev@gmail.com" }
          );

        modelBuilder.Entity<Customer>().OwnsOne(c => c.Phone)
          .HasData(
              new { CustomerId = new Guid("25b58f54-63bc-42da-8cf6-8162097e72c8"), CountryCode = "55", DDD = "11", Number = "9415452222" }
          );

        modelBuilder.Entity<Customer>()
            .HasData(
                new { Id = new Guid("9887b301-605f-46a6-93db-ac1ce8685723"), IsDeleted = false }
            );

        modelBuilder.Entity<Customer>().OwnsOne(c => c.Name)
           .HasData(
               new { CustomerId = new Guid("9887b301-605f-46a6-93db-ac1ce8685723"), FullName = "John Silva" }
           );

        modelBuilder.Entity<Customer>().OwnsOne(c => c.Document)
          .HasData(
              new { CustomerId = new Guid("9887b301-605f-46a6-93db-ac1ce8685723"), Type = DocumentType.CPF, Value = "18032939008" }
          );

        modelBuilder.Entity<Customer>().OwnsOne(c => c.Email)
          .HasData(
              new { CustomerId = new Guid("9887b301-605f-46a6-93db-ac1ce8685723"), Address = "john.silva@gmail.com" }
          );

        modelBuilder.Entity<Customer>().OwnsOne(c => c.Phone)
          .HasData(
              new { CustomerId = new Guid("9887b301-605f-46a6-93db-ac1ce8685723"), CountryCode = "55", DDD = "11", Number = "9415452222" }
          );

        modelBuilder.Entity<User>().OwnsOne(c => c.Name)
           .HasData(
               new { UserId = new Guid("fa09f3a0-f22d-40a8-9cca-0c64e5ed50e4"), FullName = "John Admin" }
           );

        modelBuilder.Entity<User>().OwnsOne(c => c.Email)
         .HasData(
             new { UserId = new Guid("fa09f3a0-f22d-40a8-9cca-0c64e5ed50e4"), Address = "john.admin@techfood.com" }
         );

        modelBuilder.Entity<User>()
            .HasData(
                // password: 123456
                new { Id = new Guid("fa09f3a0-f22d-40a8-9cca-0c64e5ed50e4"), Username = "john.admin", Role = "admin", PasswordHash = "AQAAAAIAAYagAAAAEKs0I0Zk5QKKieJTm20PwvTmpkSfnp5BhSl5E35ny8DqffCJA+CiDRnnKRCeOx8+mg==", IsDeleted = false }
            );

        modelBuilder.Entity<Category>()
            .HasData(
                new { Id = new Guid("eaa76b46-2e6b-42eb-8f5d-b213f85f25ea"), SortOrder = 0, Name = "Lanche", ImageFileName = "lanche.png", IsDeleted = false },
                new { Id = new Guid("c65e2cec-bd44-446d-8ed3-a7045cd4876a"), SortOrder = 1, Name = "Acompanhamento", ImageFileName = "acompanhamento.png", IsDeleted = false },
                new { Id = new Guid("c3a70938-9e88-437d-a801-c166d2716341"), SortOrder = 2, Name = "Bebida", ImageFileName = "bebida.png", IsDeleted = false },
                new { Id = new Guid("ec2fb26d-99a4-4eab-aa5c-7dd18d88a025"), SortOrder = 3, Name = "Sobremesa", ImageFileName = "sobremesa.png", IsDeleted = false }
            );

        modelBuilder.Entity<Product>()
            .HasData(
                new { Id = new Guid("090d8eb0-f514-4248-8512-cf0d61a262f0"), Name = "X-Burguer", Description = "Delicioso X-Burguer", Price = 19.99m, CategoryId = new Guid("eaa76b46-2e6b-42eb-8f5d-b213f85f25ea"), ImageFileName = "x-burguer.png", OutOfStock = false, IsDeleted = false },
                new { Id = new Guid("a62dc225-416a-4e36-ba35-a2bd2bbb80f7"), Name = "X-Salada", Description = "Delicioso X-Salada", Price = 21.99m, CategoryId = new Guid("eaa76b46-2e6b-42eb-8f5d-b213f85f25ea"), ImageFileName = "x-salada.png", OutOfStock = false, IsDeleted = false },
                new { Id = new Guid("3c9374f1-58e9-4b07-bdf6-73aa2f4757ff"), Name = "X-Bacon", Description = "Delicioso X-Bacon", Price = 22.99m, CategoryId = new Guid("eaa76b46-2e6b-42eb-8f5d-b213f85f25ea"), ImageFileName = "x-bacon.png", OutOfStock = false, IsDeleted = false },
                new { Id = new Guid("55f32e65-c82f-4a10-981c-cdb7b0d2715a"), Name = "Batata Frita", Description = "Crocante Batata Frita", Price = 9.99m, CategoryId = new Guid("c65e2cec-bd44-446d-8ed3-a7045cd4876a"), ImageFileName = "batata.png", OutOfStock = false, IsDeleted = false },
                new { Id = new Guid("3249b4e4-11e5-41d9-9d55-e9b1d59bfb23"), Name = "Batata Frita Grande", Description = "Crocante Batata Frita", Price = 12.99m, CategoryId = new Guid("c65e2cec-bd44-446d-8ed3-a7045cd4876a"), ImageFileName = "batata-grande.png", OutOfStock = false, IsDeleted = false },
                new { Id = new Guid("4aeb3ad6-1e06-418e-8878-e66a4ba9337f"), Name = "Nuggets de Frango", Description = "Delicioso Nuggets de Frango", Price = 13.99m, CategoryId = new Guid("c65e2cec-bd44-446d-8ed3-a7045cd4876a"), ImageFileName = "nuggets.png", OutOfStock = false, IsDeleted = false },
                new { Id = new Guid("86c50c81-c46e-4e79-a591-3b68c75cefda"), Name = "Coca-Cola", Description = "Coca-Cola", Price = 4.99m, CategoryId = new Guid("c3a70938-9e88-437d-a801-c166d2716341"), ImageFileName = "coca-cola.png", OutOfStock = false, IsDeleted = false },
                new { Id = new Guid("44c61027-8e16-444d-9f4f-e332410cccaa"), Name = "Guaraná", Description = "Guaraná", Price = 4.99m, CategoryId = new Guid("c3a70938-9e88-437d-a801-c166d2716341"), ImageFileName = "guarana.png", OutOfStock = false, IsDeleted = false },
                new { Id = new Guid("bf90f247-52cc-4bbb-b6e3-9c77b6ff546f"), Name = "Fanta", Description = "Fanta", Price = 4.99m, CategoryId = new Guid("c3a70938-9e88-437d-a801-c166d2716341"), ImageFileName = "fanta.png", OutOfStock = false, IsDeleted = false },
                new { Id = new Guid("8620cf54-0d37-4aa1-832a-eb98e9b36863"), Name = "Sprite", Description = "Sprite", Price = 4.99m, CategoryId = new Guid("c3a70938-9e88-437d-a801-c166d2716341"), ImageFileName = "sprite.png", OutOfStock = false, IsDeleted = false },
                new { Id = new Guid("de797d9f-c473-4bed-a560-e7036ca10ab1"), Name = "Milk Shake de Morango", Description = "Milk Shake de Morango", Price = 7.99m, CategoryId = new Guid("ec2fb26d-99a4-4eab-aa5c-7dd18d88a025"), ImageFileName = "milk-shake-morango.png", OutOfStock = false, IsDeleted = false },
                new { Id = new Guid("113daae6-f21f-4d38-a778-9364ac64f909"), Name = "Milk Shake de Chocolate", Description = "Milk Shake de Chocolate", Price = 7.99m, CategoryId = new Guid("ec2fb26d-99a4-4eab-aa5c-7dd18d88a025"), ImageFileName = "milk-shake-chocolate.png", OutOfStock = false, IsDeleted = false },
                new { Id = new Guid("2665c2ec-c537-4d95-9a0f-791bcd4cc938"), Name = "Milk Shake de Baunilha", Description = "Milk Shake de Baunilha", Price = 7.99m, CategoryId = new Guid("ec2fb26d-99a4-4eab-aa5c-7dd18d88a025"), ImageFileName = "milk-shake-baunilha.png", OutOfStock = false, IsDeleted = false }
                );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        optionsBuilder.LogTo(Console.WriteLine);
#endif
    }
}
