using TechFood.BackOffice.Domain.Entities;

namespace TechFood.BackOffice.Domain.Tests.Fixtures;

public static class ProductFixture
{
    public static Product CreateValid(
        string name = "X-Burguer", 
        string description = "Delicioso hambúrguer", 
        Guid? categoryId = null,
        string imageFileName = "burger.png", 
        decimal price = 19.99m)
    {
        return new Product(name, description, categoryId ?? Guid.NewGuid(), imageFileName, price);
    }

    public static Product CreateXBurguer(Guid? categoryId = null)
    {
        return new Product("X-Burguer", "Delicioso hambúrguer com carne", categoryId ?? Guid.NewGuid(), "x-burger.png", 19.99m);
    }

    public static Product CreateXSalada(Guid? categoryId = null)
    {
        return new Product("X-Salada", "Hambúrguer com salada fresca", categoryId ?? Guid.NewGuid(), "x-salada.png", 21.99m);
    }

    public static Product CreateBatataFrita(Guid? categoryId = null)
    {
        return new Product("Batata Frita", "Batatas crocantes", categoryId ?? Guid.NewGuid(), "batata.png", 9.99m);
    }

    public static Product CreateCocaCola(Guid? categoryId = null)
    {
        return new Product("Coca-Cola", "Refrigerante gelado", categoryId ?? Guid.NewGuid(), "coca-cola.png", 4.99m);
    }

    public static Product CreateMilkShake(Guid? categoryId = null)
    {
        return new Product("Milk Shake de Chocolate", "Cremoso milk shake", categoryId ?? Guid.NewGuid(), "milkshake.png", 7.99m);
    }

    public static List<Product> CreateMultiple(Guid? categoryId = null)
    {
        var defaultCategoryId = categoryId ?? Guid.NewGuid();
        return new List<Product>
        {
            CreateXBurguer(defaultCategoryId),
            CreateXSalada(defaultCategoryId),
            CreateBatataFrita(defaultCategoryId),
            CreateCocaCola(defaultCategoryId),
            CreateMilkShake(defaultCategoryId)
        };
    }

    public static Product CreateWithPrice(decimal price)
    {
        return new Product("Test Product", "Test Description", Guid.NewGuid(), "test.png", price);
    }

    public static Product CreateOutOfStock()
    {
        var product = CreateValid();
        product.SetOutOfStock(true);
        return product;
    }

    public static Product CreateInStock()
    {
        var product = CreateValid();
        product.SetOutOfStock(false);
        return product;
    }
}