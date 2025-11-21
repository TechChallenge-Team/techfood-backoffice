using FluentAssertions;
using NSubstitute;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Repositories;
using Xunit;

namespace TechFood.BackOffice.Infra.Tests.Repositories;

public class CategoryRepositoryTests
{
    private readonly ICategoryRepository _mockRepository;

    public CategoryRepositoryTests()
    {
        _mockRepository = Substitute.For<ICategoryRepository>();
    }

    [Fact]
    public async Task AddAsync_ShouldReturnCategoryId()
    {
        // Arrange
        var category = new Category("Test Category", "test.jpg", 1);
        var expectedId = category.Id;
        
        _mockRepository.AddAsync(category).Returns(expectedId);

        // Act
        var result = await _mockRepository.AddAsync(category);

        // Assert
        result.Should().Be(expectedId);
        await _mockRepository.Received(1).AddAsync(category);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOrderedCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category("Category A", "a.jpg", 1),
            new Category("Category B", "b.jpg", 2),
            new Category("Category C", "c.jpg", 3)
        };

        _mockRepository.GetAllAsync().Returns(categories);

        // Act
        var result = await _mockRepository.GetAllAsync();

        // Assert
        result.Should().HaveCount(3);
        result.Should().BeEquivalentTo(categories);
        await _mockRepository.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnCategory()
    {
        // Arrange
        var category = new Category("Test Category", "test.jpg", 1);
        var categoryId = category.Id;
        
        _mockRepository.GetByIdAsync(categoryId).Returns(category);

        // Act
        var result = await _mockRepository.GetByIdAsync(categoryId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(categoryId);
        result.Name.Should().Be("Test Category");
        await _mockRepository.Received(1).GetByIdAsync(categoryId);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();
        _mockRepository.GetByIdAsync(nonExistingId).Returns((Category?)null);

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
        var category = new Category("To Delete", "delete.jpg", 1);

        // Act
        await _mockRepository.DeleteAsync(category);

        // Assert
        await _mockRepository.Received(1).DeleteAsync(category);
    }
}