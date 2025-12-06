using TechFood.BackOffice.Application.Menu.Dto;
using TechFood.BackOffice.Application.Menu.Queries;
using TechFood.BackOffice.Application.Menu.Queries.GetMenu;

namespace TechFood.BackOffice.Application.Tests.Queries;

public class GetMenuQueryHandlerTests
{
    private readonly Mock<IMenuQueryProvider> _menuQueryProviderMock;
    private readonly GetMenuQuery.Handler _handler;

    public GetMenuQueryHandlerTests()
    {
        _menuQueryProviderMock = new Mock<IMenuQueryProvider>();
        _handler = new GetMenuQuery.Handler(_menuQueryProviderMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCompleteMenu()
    {
        // Arrange
        var query = new GetMenuQuery();
        var expectedMenu = new MenuDto
        {
            Categories = new List<CategoryDto>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Lanche",
                    ImageUrl = "lanche.png",
                    SortOrder = 0,
                    Products = new List<ProductDto>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            CategoryId = Guid.NewGuid(),
                            Name = "X-Burguer",
                            Description = "Delicioso hambúrguer",
                            Price = 19.99m,
                            ImageUrl = "burger.png"
                        }
                    }
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Bebida",
                    ImageUrl = "bebida.png",
                    SortOrder = 1,
                    Products = new List<ProductDto>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            CategoryId = Guid.NewGuid(),
                            Name = "Coca-Cola",
                            Description = "Refrigerante",
                            Price = 4.99m,
                            ImageUrl = "coca.png"
                        }
                    }
                }
            }
        };

        _menuQueryProviderMock.Setup(p => p.GetAsync())
                              .ReturnsAsync(expectedMenu);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedMenu);
        result.Categories.Should().HaveCount(2);
        result.Categories.First().Products.Should().HaveCount(1);
        _menuQueryProviderMock.Verify(p => p.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenMenuIsEmpty_ShouldReturnEmptyMenu()
    {
        // Arrange
        var query = new GetMenuQuery();
        var emptyMenu = new MenuDto { Categories = new List<CategoryDto>() };

        _menuQueryProviderMock.Setup(p => p.GetAsync())
                              .ReturnsAsync(emptyMenu);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Categories.Should().BeEmpty();
        _menuQueryProviderMock.Verify(p => p.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ProviderThrowsException_ShouldPropagateException()
    {
        // Arrange
        var query = new GetMenuQuery();
        var expectedException = new Exception("Database connection error");

        _menuQueryProviderMock.Setup(p => p.GetAsync())
                              .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
        exception.Should().Be(expectedException);
    }

    [Fact]
    public async Task Handle_ShouldCallProviderOnce()
    {
        // Arrange
        var query = new GetMenuQuery();
        var menu = new MenuDto { Categories = new List<CategoryDto>() };

        _menuQueryProviderMock.Setup(p => p.GetAsync())
                              .ReturnsAsync(menu);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _menuQueryProviderMock.Verify(p => p.GetAsync(), Times.Once);
        _menuQueryProviderMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldReturnCategoriesInSortOrder()
    {
        // Arrange
        var query = new GetMenuQuery();
        var menu = new MenuDto
        {
            Categories = new List<CategoryDto>
            {
                new() { Id = Guid.NewGuid(), Name = "Lanche", SortOrder = 0, Products = new List<ProductDto>() },
                new() { Id = Guid.NewGuid(), Name = "Bebida", SortOrder = 2, Products = new List<ProductDto>() },
                new() { Id = Guid.NewGuid(), Name = "Sobremesa", SortOrder = 3, Products = new List<ProductDto>() }
            }
        };

        _menuQueryProviderMock.Setup(p => p.GetAsync())
                              .ReturnsAsync(menu);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Categories.Should().BeInAscendingOrder(c => c.SortOrder);
    }
}