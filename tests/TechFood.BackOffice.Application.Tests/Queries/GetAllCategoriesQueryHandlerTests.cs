using TechFood.BackOffice.Application.Categories.Dto;
using TechFood.BackOffice.Application.Categories.Queries;
using TechFood.BackOffice.Application.Categories.Queries.ListCategories;

namespace TechFood.BackOffice.Application.Tests.Queries;

public class ListCategoriesQueryHandlerTests
{
    private readonly Mock<ICategoryQueryProvider> _categoryQueryProviderMock;
    private readonly ListCategoriesQueryHandler _handler;

    public ListCategoriesQueryHandlerTests()
    {
        _categoryQueryProviderMock = new Mock<ICategoryQueryProvider>();
        _handler = new ListCategoriesQueryHandler(_categoryQueryProviderMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllCategories()
    {
        // Arrange
        var query = new ListCategoriesQuery();
        var expectedCategories = new List<CategoryDto>
        {
            new() { Id = Guid.NewGuid(), Name = "Lanche", ImageUrl = "lanche.png" },
            new() { Id = Guid.NewGuid(), Name = "Bebida", ImageUrl = "bebida.png" }
        };

        _categoryQueryProviderMock.Setup(p => p.GetAllAsync())
                                  .ReturnsAsync(expectedCategories);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedCategories);
        _categoryQueryProviderMock.Verify(p => p.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNoCategoriesExist_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new ListCategoriesQuery();
        var emptyList = new List<CategoryDto>();

        _categoryQueryProviderMock.Setup(p => p.GetAllAsync())
                                  .ReturnsAsync(emptyList);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
        _categoryQueryProviderMock.Verify(p => p.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ProviderThrowsException_ShouldPropagateException()
    {
        // Arrange
        var query = new ListCategoriesQuery();
        var expectedException = new Exception("Database connection error");

        _categoryQueryProviderMock.Setup(p => p.GetAllAsync())
                                  .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
        exception.Should().Be(expectedException);
    }

    [Fact]
    public async Task Handle_ShouldCallProviderOnce()
    {
        // Arrange
        var query = new ListCategoriesQuery();
        var categories = new List<CategoryDto>();

        _categoryQueryProviderMock.Setup(p => p.GetAllAsync())
                                  .ReturnsAsync(categories);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _categoryQueryProviderMock.Verify(p => p.GetAllAsync(), Times.Once);
        _categoryQueryProviderMock.VerifyNoOtherCalls();
    }
}