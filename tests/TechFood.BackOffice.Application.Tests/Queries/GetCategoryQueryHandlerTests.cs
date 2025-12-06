using TechFood.BackOffice.Application.Categories.Dto;
using TechFood.BackOffice.Application.Categories.Queries;
using TechFood.BackOffice.Application.Categories.Queries.GetCategory;

namespace TechFood.BackOffice.Application.Tests.Queries;

public class GetCategoryQueryHandlerTests
{
    private readonly Mock<ICategoryQueryProvider> _categoryQueryProviderMock;
    private readonly GetCategoryQueryHandler _handler;

    public GetCategoryQueryHandlerTests()
    {
        _categoryQueryProviderMock = new Mock<ICategoryQueryProvider>();
        _handler = new GetCategoryQueryHandler(_categoryQueryProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoryExists_ShouldReturnCategoryDto()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var expectedCategory = new CategoryDto
        {
            Id = categoryId,
            Name = "Lanche",
            ImageUrl = "lanche.png"
        };

        var query = new GetCategoryQuery(categoryId);

        _categoryQueryProviderMock
            .Setup(q => q.GetByIdAsync(categoryId))
            .ReturnsAsync(expectedCategory);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(categoryId);
        result.Name.Should().Be("Lanche");
        result.ImageUrl.Should().Be("lanche.png");

        _categoryQueryProviderMock.Verify(
            q => q.GetByIdAsync(categoryId),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCategoryDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var query = new GetCategoryQuery(categoryId);

        _categoryQueryProviderMock
            .Setup(q => q.GetByIdAsync(categoryId))
            .ReturnsAsync((CategoryDto?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();

        _categoryQueryProviderMock.Verify(
            q => q.GetByIdAsync(categoryId),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCallQueryProviderWithCorrectId()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var query = new GetCategoryQuery(categoryId);

        _categoryQueryProviderMock
            .Setup(q => q.GetByIdAsync(categoryId))
            .ReturnsAsync((CategoryDto?)null);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _categoryQueryProviderMock.Verify(
            q => q.GetByIdAsync(It.Is<Guid>(id => id == categoryId)),
            Times.Once);
    }

    [Theory]
    [InlineData("Lanches", "lanches.png")]
    [InlineData("Bebidas", "bebidas.png")]
    [InlineData("Sobremesas", "sobremesas.png")]
    [InlineData("Acompanhamentos", "acompanhamentos.png")]
    public async Task Handle_WithDifferentCategories_ShouldReturnCorrectDto(string name, string imageUrl)
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var expectedCategory = new CategoryDto
        {
            Id = categoryId,
            Name = name,
            ImageUrl = imageUrl
        };

        var query = new GetCategoryQuery(categoryId);

        _categoryQueryProviderMock
            .Setup(q => q.GetByIdAsync(categoryId))
            .ReturnsAsync(expectedCategory);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(name);
        result.ImageUrl.Should().Be(imageUrl);
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldComplete()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var query = new GetCategoryQuery(categoryId);
        var cancellationToken = new CancellationToken();

        _categoryQueryProviderMock
            .Setup(q => q.GetByIdAsync(categoryId))
            .ReturnsAsync((CategoryDto?)null);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().BeNull();
        _categoryQueryProviderMock.Verify(
            q => q.GetByIdAsync(categoryId),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenQueryProviderThrowsException_ShouldPropagateException()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var query = new GetCategoryQuery(categoryId);

        _categoryQueryProviderMock
            .Setup(q => q.GetByIdAsync(categoryId))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            async () => await _handler.Handle(query, CancellationToken.None));

        _categoryQueryProviderMock.Verify(
            q => q.GetByIdAsync(categoryId),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptyGuid_ShouldCallQueryProvider()
    {
        // Arrange
        var categoryId = Guid.Empty;
        var query = new GetCategoryQuery(categoryId);

        _categoryQueryProviderMock
            .Setup(q => q.GetByIdAsync(categoryId))
            .ReturnsAsync((CategoryDto?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _categoryQueryProviderMock.Verify(
            q => q.GetByIdAsync(Guid.Empty),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnDtoWithAllProperties()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var expectedCategory = new CategoryDto
        {
            Id = categoryId,
            Name = "Bebidas",
            ImageUrl = "https://example.com/bebidas.png"
        };

        var query = new GetCategoryQuery(categoryId);

        _categoryQueryProviderMock
            .Setup(q => q.GetByIdAsync(categoryId))
            .ReturnsAsync(expectedCategory);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedCategory);
    }
}
