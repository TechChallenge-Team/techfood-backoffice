using MediatR;
using TechFood.Api.Controllers;
using TechFood.BackOffice.Application.Menu.Dto;
using TechFood.BackOffice.Application.Menu.Queries.GetMenu;

namespace TechFood.BackOffice.Api.Tests.Controllers;

public class MenuControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly MenuController _controller;

    public MenuControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new MenuController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnOkResult_WithMenu()
    {
        // Arrange
        var menu = new MenuDto
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
                }
            }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetMenuQuery>(), default))
                     .ReturnsAsync(menu);

        // Act
        var result = await _controller.GetAsync();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(menu);
    }

    [Fact]
    public async Task GetAsync_ShouldCallMediatorWithGetMenuQuery()
    {
        // Arrange
        var menu = new MenuDto { Categories = new List<CategoryDto>() };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetMenuQuery>(), default))
                     .ReturnsAsync(menu);

        // Act
        await _controller.GetAsync();

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetMenuQuery>(), default), Times.Once);
    }

    [Fact]
    public async Task GetAsync_WhenMenuIsEmpty_ShouldReturnOkWithEmptyCategories()
    {
        // Arrange
        var emptyMenu = new MenuDto { Categories = new List<CategoryDto>() };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetMenuQuery>(), default))
                     .ReturnsAsync(emptyMenu);

        // Act
        var result = await _controller.GetAsync();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var returnedMenu = okResult!.Value as MenuDto;
        returnedMenu!.Categories.Should().BeEmpty();
    }
}