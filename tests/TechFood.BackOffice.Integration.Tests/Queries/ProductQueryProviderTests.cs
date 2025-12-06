using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Infra.Persistence.Contexts;
using TechFood.BackOffice.Infra.Persistence.Queries;
using TechFood.Shared.Infra.Extensions;

namespace TechFood.BackOffice.Integration.Tests.Queries;

public class ProductQueryProviderTests : IDisposable
{
    private readonly BackOfficeContext _context;
    private readonly Mock<IImageUrlResolver> _imageUrlResolverMock;
    private readonly ProductQueryProvider _queryProvider;
    private readonly Faker _faker;

    public ProductQueryProviderTests()
    {
        var infraOptions = Options.Create(new InfraOptions
        {
            InfraAssembly = typeof(BackOfficeContext).Assembly
        });

        var options = new DbContextOptionsBuilder<BackOfficeContext>()
            .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new BackOfficeContext(infraOptions, options);
        _imageUrlResolverMock = new Mock<IImageUrlResolver>();
        _imageUrlResolverMock
            .Setup(x => x.BuildFilePath(It.IsAny<string>(), It.IsAny<string>()))
            .Returns<string, string>((folder, filename) => $"/images/{folder}/{filename}");

        _queryProvider = new ProductQueryProvider(_context, _imageUrlResolverMock.Object);
        _faker = new Faker();
    }

    [Fact(DisplayName = "Should return all products")]
    [Trait("Integration", "ProductQueryProvider")]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var category = new Category("Lanches", "lanches.jpg", 1);
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        var categoryId = category.Id;

        var product1 = new Product(
            "X-Burger",
            "Hambúrguer com queijo",
            categoryId,
            "xburger.jpg",
            25.00m);

        var product2 = new Product(
            "X-Salad",
            "Hambúrguer com salada",
            categoryId,
            "xsalad.jpg",
            28.00m);

        await _context.Products.AddRangeAsync(product1, product2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _queryProvider.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.Name == "X-Burger");
        result.Should().Contain(p => p.Name == "X-Salad");
    }

    [Fact(DisplayName = "Should return product by id")]
    [Trait("Integration", "ProductQueryProvider")]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenExists()
    {
        // Arrange
        var category = new Category("Bebidas", "bebidas.jpg", 1);
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        var categoryId = category.Id;

        var product = new Product(
            "Coca-Cola",
            "Refrigerante 350ml",
            categoryId,
            "coca.jpg",
            5.00m);

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        var productId = product.Id;

        // Act
        var result = await _queryProvider.GetByIdAsync(productId);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Coca-Cola");
        result.Description.Should().Be("Refrigerante 350ml");
        result.Price.Should().Be(5.00m);
        result.CategoryId.Should().Be(categoryId);
    }

    [Fact(DisplayName = "Should return null when product does not exist")]
    [Trait("Integration", "ProductQueryProvider")]
    public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _queryProvider.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact(DisplayName = "Should return empty list when no products exist")]
    [Trait("Integration", "ProductQueryProvider")]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        // Act
        var result = await _queryProvider.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact(DisplayName = "Should include correct image URL for products")]
    [Trait("Integration", "ProductQueryProvider")]
    public async Task GetAllAsync_ShouldIncludeCorrectImageUrl()
    {
        // Arrange
        var category = new Category("Sobremesas", "sobremesas.jpg", 1);
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        var categoryId = category.Id;
        var imageFileName = "pudim.jpg";
        var product = new Product(
            "Pudim",
            "Pudim de leite condensado",
            categoryId,
            imageFileName,
            8.00m);

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        // Act
        var result = await _queryProvider.GetAllAsync();

        // Assert
        var productDto = result.Single();
        productDto.ImageUrl.Should().Be($"/images/product/{imageFileName}");
        _imageUrlResolverMock.Verify(x => x.BuildFilePath("product", imageFileName), Times.Once);
    }

    [Fact(DisplayName = "Should return products with correct out of stock status")]
    [Trait("Integration", "ProductQueryProvider")]
    public async Task GetAllAsync_ShouldReturnProducts_WithCorrectOutOfStockStatus()
    {
        // Arrange
        var category = new Category("Lanches", "lanches.jpg", 1);
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        var categoryId = category.Id;

        var inStockProduct = new Product(
            "X-Bacon",
            "Hambúrguer com bacon",
            categoryId,
            "xbacon.jpg",
            30.00m);

        var outOfStockProduct = new Product(
            "X-Frango",
            "Hambúrguer de frango",
            categoryId,
            "xfrango.jpg",
            27.00m);

        outOfStockProduct.SetOutOfStock(true);

        await _context.Products.AddRangeAsync(inStockProduct, outOfStockProduct);
        await _context.SaveChangesAsync();

        // Act
        var result = await _queryProvider.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);

        var inStock = result.First(p => p.Name == "X-Bacon");
        inStock.OutOfStock.Should().BeFalse();

        var outOfStock = result.First(p => p.Name == "X-Frango");
        outOfStock.OutOfStock.Should().BeTrue();
    }

    [Fact(DisplayName = "Should return products from different categories")]
    [Trait("Integration", "ProductQueryProvider")]
    public async Task GetAllAsync_ShouldReturnProducts_FromDifferentCategories()
    {
        // Arrange
        var category1 = new Category("Lanches", "lanches.jpg", 1);
        var category2 = new Category("Bebidas", "bebidas.jpg", 2);

        await _context.Categories.AddRangeAsync(category1, category2);
        await _context.SaveChangesAsync();

        var category1Id = category1.Id;
        var category2Id = category2.Id;

        var product1 = new Product(
            "X-Burger",
            "Hambúrguer",
            category1Id,
            "xburger.jpg",
            25.00m);

        var product2 = new Product(
            "Suco",
            "Suco natural",
            category2Id,
            "suco.jpg",
            7.00m);

        await _context.Products.AddRangeAsync(product1, product2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _queryProvider.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.CategoryId == category1Id);
        result.Should().Contain(p => p.CategoryId == category2Id);
    }

    [Fact(DisplayName = "Should return products with correct price")]
    [Trait("Integration", "ProductQueryProvider")]
    public async Task GetAllAsync_ShouldReturnProducts_WithCorrectPrice()
    {
        // Arrange
        var category = new Category("Pizzas", "pizzas.jpg", 1);
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        var categoryId = category.Id;

        var product = new Product(
            "Pizza Margherita",
            "Pizza com molho de tomate e mussarela",
            categoryId,
            "margherita.jpg",
            35.50m);

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        // Act
        var result = await _queryProvider.GetAllAsync();

        // Assert
        var productDto = result.Single();
        productDto.Price.Should().Be(35.50m);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
