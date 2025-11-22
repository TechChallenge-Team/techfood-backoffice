using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Infra.Persistence.Contexts;
using TechFood.BackOffice.Infra.Persistence.Queries;
using TechFood.Shared.Infra.Extensions;

namespace TechFood.BackOffice.Integration.Tests.Queries;

public class CategoryQueryProviderTests : IDisposable
{
    private readonly BackOfficeContext _context;
    private readonly Mock<IImageUrlResolver> _imageUrlResolverMock;
    private readonly CategoryQueryProvider _queryProvider;
    private readonly Faker _faker;

    public CategoryQueryProviderTests()
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

        _queryProvider = new CategoryQueryProvider(_context, _imageUrlResolverMock.Object);
        _faker = new Faker();
    }

    [Fact(DisplayName = "Should return all categories")]
    [Trait("Integration", "CategoryQueryProvider")]
    public async Task GetAllAsync_ShouldReturnAllCategories()
    {
        // Arrange
        var category1 = new Category("Lanches", "lanches.jpg", 1);
        var category2 = new Category("Bebidas", "bebidas.jpg", 2);
        var category3 = new Category("Sobremesas", "sobremesas.jpg", 3);

        await _context.Categories.AddRangeAsync(category1, category2, category3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _queryProvider.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Should().Contain(c => c.Name == "Lanches");
        result.Should().Contain(c => c.Name == "Bebidas");
        result.Should().Contain(c => c.Name == "Sobremesas");
    }

    [Fact(DisplayName = "Should return category by id")]
    [Trait("Integration", "CategoryQueryProvider")]
    public async Task GetByIdAsync_ShouldReturnCategory_WhenExists()
    {
        // Arrange
        var category = new Category("Acompanhamentos", "acompanhamentos.jpg", 1);
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        var categoryId = category.Id;

        // Act
        var result = await _queryProvider.GetByIdAsync(categoryId);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Acompanhamentos");
        result.ImageUrl.Should().Be("/images/category/acompanhamentos.jpg");
    }

    [Fact(DisplayName = "Should return null when category does not exist")]
    [Trait("Integration", "CategoryQueryProvider")]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _queryProvider.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact(DisplayName = "Should return empty list when no categories exist")]
    [Trait("Integration", "CategoryQueryProvider")]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoCategoriesExist()
    {
        // Act
        var result = await _queryProvider.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact(DisplayName = "Should include correct image URL for categories")]
    [Trait("Integration", "CategoryQueryProvider")]
    public async Task GetAllAsync_ShouldIncludeCorrectImageUrl()
    {
        // Arrange
        var imageFileName = "pizzas.jpg";
        var category = new Category("Pizzas", imageFileName, 1);

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        // Act
        var result = await _queryProvider.GetAllAsync();

        // Assert
        var categoryDto = result.Single();
        categoryDto.ImageUrl.Should().Be($"/images/category/{imageFileName}");
        _imageUrlResolverMock.Verify(x => x.BuildFilePath("category", imageFileName), Times.Once);
    }

    [Fact(DisplayName = "Should return categories ordered by sort order")]
    [Trait("Integration", "CategoryQueryProvider")]
    public async Task GetAllAsync_ShouldReturnCategories_OrderedBySortOrder()
    {
        // Arrange
        var category1 = new Category("Zebras", "z.jpg", 3);
        var category2 = new Category("Acompanhamentos", "a.jpg", 1);
        var category3 = new Category("Lanches", "l.jpg", 2);

        await _context.Categories.AddRangeAsync(category1, category2, category3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _queryProvider.GetAllAsync();

        // Assert
        result.Should().HaveCount(3);
        result.First().Name.Should().Be("Acompanhamentos");
        result.Skip(1).First().Name.Should().Be("Lanches");
        result.Last().Name.Should().Be("Zebras");
    }

    [Fact(DisplayName = "Should return category with all properties populated")]
    [Trait("Integration", "CategoryQueryProvider")]
    public async Task GetByIdAsync_ShouldReturnCategory_WithAllPropertiesPopulated()
    {
        // Arrange
        var name = "Sucos Naturais";
        var imageFileName = "sucos.jpg";
        var sortOrder = 5;
        var category = new Category(name, imageFileName, sortOrder);

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        var categoryId = category.Id;

        // Act
        var result = await _queryProvider.GetByIdAsync(categoryId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(categoryId);
        result.Name.Should().Be(name);
        result.ImageUrl.Should().Contain(imageFileName);
    }

    [Fact(DisplayName = "Should handle categories with special characters in name")]
    [Trait("Integration", "CategoryQueryProvider")]
    public async Task GetAllAsync_ShouldHandleCategories_WithSpecialCharactersInName()
    {
        // Arrange
        var category1 = new Category("Café & Chá", "cafe.jpg", 1);
        var category2 = new Category("Açaí", "acai.jpg", 2);

        await _context.Categories.AddRangeAsync(category1, category2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _queryProvider.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(c => c.Name == "Café & Chá");
        result.Should().Contain(c => c.Name == "Açaí");
    }

    [Fact(DisplayName = "Should retrieve multiple categories efficiently")]
    [Trait("Integration", "CategoryQueryProvider")]
    public async Task GetAllAsync_WithManyCategories_ShouldReturnAll()
    {
        // Arrange
        var categories = Enumerable.Range(1, 10)
            .Select(i => new Category($"Category {i}", $"image{i}.jpg", i))
            .ToList();

        await _context.Categories.AddRangeAsync(categories);
        await _context.SaveChangesAsync();

        // Act
        var result = await _queryProvider.GetAllAsync();

        // Assert
        result.Should().HaveCount(10);
        result.Select(c => c.Name).Should().Contain(categories.Select(c => c.Name));
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
