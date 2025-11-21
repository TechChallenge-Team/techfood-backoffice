using FluentAssertions;
using NSubstitute;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Repositories;
using Xunit;

namespace TechFood.BackOffice.Infra.Tests.Repositories;

public class ProductRepositoryTests
{
    private readonly IProductRepository _mockRepository;

    public ProductRepositoryTests()
    {
        _mockRepository = Substitute.For<IProductRepository>();
    }

    [Fact]
    public async Task AddAsync_ShouldReturnProductId()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var product = new Product("Test Product", "Test Description", categoryId, "test-product.jpg", 25.99m);
        var expectedId = product.Id;
        
        _mockRepository.AddAsync(product).Returns(expectedId);

        // Act
        var result = await _mockRepository.AddAsync(product);

        // Assert
        result.Should().Be(expectedId);
        await _mockRepository.Received(1).AddAsync(product);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var products = new List<Product>
        {
            new Product("Product 1", "Description 1", categoryId, "product1.jpg", 10.99m),
            new Product("Product 2", "Description 2", categoryId, "product2.jpg", 15.99m)
        };

        _mockRepository.GetAllAsync().Returns(products);

        // Act
        var result = await _mockRepository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(products);
        await _mockRepository.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnProduct()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var product = new Product("Test Product", "Test Description", categoryId, "test-product.jpg", 25.99m);
        var productId = product.Id;
        
        _mockRepository.GetByIdAsync(productId).Returns(product);

        // Act
        var result = await _mockRepository.GetByIdAsync(productId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(productId);
        result.Name.Should().Be("Test Product");
        result.Description.Should().Be("Test Description");
        result.Price.Should().Be(25.99m);
        await _mockRepository.Received(1).GetByIdAsync(productId);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();
        _mockRepository.GetByIdAsync(nonExistingId).Returns((Product?)null);

        // Act
        var result = await _mockRepository.GetByIdAsync(nonExistingId);

        // Assert
        result.Should().BeNull();
        await _mockRepository.Received(1).GetByIdAsync(nonExistingId);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallRepositoryDelete()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var product = new Product("To Delete", "Will be deleted", categoryId, "delete.jpg", 10.00m);

        // Act
        await _mockRepository.DeleteAsync(product);

        // Assert
        await _mockRepository.Received(1).DeleteAsync(product);
    }
}