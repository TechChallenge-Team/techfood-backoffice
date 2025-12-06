using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.EntityFrameworkCore.Extensions;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Enums;
using TechFood.BackOffice.Domain.ValueObjects;
using TechFood.Shared.Domain.Entities;
using TechFood.Shared.Infra.Extensions;
using TechFood.Shared.Infra.Persistence.Contexts;

namespace TechFood.BackOffice.Infra.Persistence.Contexts;

public class BackOfficeContext(
    IOptions<InfraOptions> infraOptions,
    DbContextOptions<BackOfficeContext> options
        ) : TechFoodContext(infraOptions, options)
{

    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<Customer> Customers { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
    }

    public async Task SeedDataAsync()
    {
        // Check if data already exists
        if (await Categories.AnyAsync() || await Products.AnyAsync() || await Users.AnyAsync() || await Customers.AnyAsync())
            return;

        // Temporarily disable transactions for seeding (MongoDB standalone doesn't support transactions)
        var originalBehavior = Database.AutoTransactionBehavior;
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;

        try
        {
            // Seed Customers
            var customers = new[]
            {
            CreateCustomerWithId(
                new Guid("25b58f54-63bc-42da-8cf6-8162097e72c8"),
                "John", "john.dev@gmail.com",
                DocumentType.CPF, "63585272070",
                "55", "11", "9415452222"
            ),
            CreateCustomerWithId(
                new Guid("9887b301-605f-46a6-93db-ac1ce8685723"),
                "John Silva", "john.silva@gmail.com",
                DocumentType.CPF, "18032939008",
                "55", "11", "9415452222"
            )
        };

            // Seed Users
            var users = new[]
            {
            CreateUserWithId(
                new Guid("fa09f3a0-f22d-40a8-9cca-0c64e5ed50e4"),
                "John Admin", "john.admin@techfood.com",
                "john.admin", "admin",
                "AQAAAAIAAYagAAAAEKs0I0Zk5QKKieJTm20PwvTmpkSfnp5BhSl5E35ny8DqffCJA+CiDRnnKRCeOx8+mg=="
            )
        };

            // Seed Categories  
            var categories = new[]
            {
            CreateCategoryWithId(new Guid("eaa76b46-2e6b-42eb-8f5d-b213f85f25ea"), "Lanche", "lanche.png", 0),
            CreateCategoryWithId(new Guid("c65e2cec-bd44-446d-8ed3-a7045cd4876a"), "Acompanhamento", "acompanhamento.png", 1),
            CreateCategoryWithId(new Guid("c3a70938-9e88-437d-a801-c166d2716341"), "Bebida", "bebida.png", 2),
            CreateCategoryWithId(new Guid("ec2fb26d-99a4-4eab-aa5c-7dd18d88a025"), "Sobremesa", "sobremesa.png", 3)
        };

            // Seed Products
            var products = new[]
            {
            CreateProductWithId(new Guid("090d8eb0-f514-4248-8512-cf0d61a262f0"), "X-Burguer", "Delicioso X-Burguer", 19.99m, new Guid("eaa76b46-2e6b-42eb-8f5d-b213f85f25ea"), "x-burguer.png"),
            CreateProductWithId(new Guid("a62dc225-416a-4e36-ba35-a2bd2bbb80f7"), "X-Salada", "Delicioso X-Salada", 21.99m, new Guid("eaa76b46-2e6b-42eb-8f5d-b213f85f25ea"), "x-salada.png"),
            CreateProductWithId(new Guid("3c9374f1-58e9-4b07-bdf6-73aa2f4757ff"), "X-Bacon", "Delicioso X-Bacon", 22.99m, new Guid("eaa76b46-2e6b-42eb-8f5d-b213f85f25ea"), "x-bacon.png"),
            CreateProductWithId(new Guid("55f32e65-c82f-4a10-981c-cdb7b0d2715a"), "Batata Frita", "Crocante Batata Frita", 9.99m, new Guid("c65e2cec-bd44-446d-8ed3-a7045cd4876a"), "batata.png"),
            CreateProductWithId(new Guid("3249b4e4-11e5-41d9-9d55-e9b1d59bfb23"), "Batata Frita Grande", "Crocante Batata Frita", 12.99m, new Guid("c65e2cec-bd44-446d-8ed3-a7045cd4876a"), "batata-grande.png"),
            CreateProductWithId(new Guid("4aeb3ad6-1e06-418e-8878-e66a4ba9337f"), "Nuggets de Frango", "Delicioso Nuggets de Frango", 13.99m, new Guid("c65e2cec-bd44-446d-8ed3-a7045cd4876a"), "nuggets.png"),
            CreateProductWithId(new Guid("86c50c81-c46e-4e79-a591-3b68c75cefda"), "Coca-Cola", "Coca-Cola", 4.99m, new Guid("c3a70938-9e88-437d-a801-c166d2716341"), "coca-cola.png"),
            CreateProductWithId(new Guid("44c61027-8e16-444d-9f4f-e332410cccaa"), "Guaraná", "Guaraná", 4.99m, new Guid("c3a70938-9e88-437d-a801-c166d2716341"), "guarana.png"),
            CreateProductWithId(new Guid("bf90f247-52cc-4bbb-b6e3-9c77b6ff546f"), "Fanta", "Fanta", 4.99m, new Guid("c3a70938-9e88-437d-a801-c166d2716341"), "fanta.png"),
            CreateProductWithId(new Guid("8620cf54-0d37-4aa1-832a-eb98e9b36863"), "Sprite", "Sprite", 4.99m, new Guid("c3a70938-9e88-437d-a801-c166d2716341"), "sprite.png"),
            CreateProductWithId(new Guid("de797d9f-c473-4bed-a560-e7036ca10ab1"), "Milk Shake de Morango", "Milk Shake de Morango", 7.99m, new Guid("ec2fb26d-99a4-4eab-aa5c-7dd18d88a025"), "milk-shake-morango.png"),
            CreateProductWithId(new Guid("113daae6-f21f-4d38-a778-9364ac64f909"), "Milk Shake de Chocolate", "Milk Shake de Chocolate", 7.99m, new Guid("ec2fb26d-99a4-4eab-aa5c-7dd18d88a025"), "milk-shake-chocolate.png"),
            CreateProductWithId(new Guid("2665c2ec-c537-4d95-9a0f-791bcd4cc938"), "Milk Shake de Baunilha", "Milk Shake de Baunilha", 7.99m, new Guid("ec2fb26d-99a4-4eab-aa5c-7dd18d88a025"), "milk-shake-baunilha.png")
        };

            await Customers.AddRangeAsync(customers);
            await Users.AddRangeAsync(users);
            await Categories.AddRangeAsync(categories);
            await Products.AddRangeAsync(products);

            await SaveChangesAsync();
        }
        finally
        {
            // Restore original transaction behavior
            Database.AutoTransactionBehavior = originalBehavior;
        }
    }

    private static Customer CreateCustomerWithId(Guid id, string name, string email, DocumentType docType, string docValue, string countryCode, string ddd, string number)
    {
        var customer = new Customer(new Name(name), new Email(email), new Document(docType, docValue), new Phone(countryCode, ddd, number));
        SetEntityId(customer, id);
        return customer;
    }

    private static User CreateUserWithId(Guid id, string name, string email, string username, string role, string passwordHash)
    {
        var user = new User(new Name(name), username, role, new Email(email));
        user.SetPassword(passwordHash);
        SetEntityId(user, id);
        return user;
    }

    private static Category CreateCategoryWithId(Guid id, string name, string imageFileName, int sortOrder)
    {
        var category = new Category(name, imageFileName, sortOrder);
        SetEntityId(category, id);
        return category;
    }

    private static Product CreateProductWithId(Guid id, string name, string description, decimal price, Guid categoryId, string imageFileName)
    {
        var product = new Product(name, description, categoryId, imageFileName, price);
        SetEntityId(product, id);
        return product;
    }

    private static void SetEntityId(Entity entity, Guid id)
    {
        var idProperty = entity.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (idProperty != null)
        {
            // Force set the value even if it has a private setter
            idProperty.SetValue(entity, id);
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        optionsBuilder.LogTo(Console.WriteLine);
#endif
    }
}
