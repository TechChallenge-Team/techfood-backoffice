using MediatR;
using Microsoft.AspNetCore.Http;
using TechFood.Api.Controllers;
using TechFood.BackOffice.Application.Categories.Commands.CreateCategory;
using TechFood.BackOffice.Application.Categories.Commands.DeleteCategory;
using TechFood.BackOffice.Application.Categories.Dto;
using TechFood.BackOffice.Application.Categories.Queries.ListCategories;
using TechFood.BackOffice.Application.Categories.Queries.GetCategory;
using TechFood.BackOffice.Contracts.Categories;

namespace TechFood.BackOffice.Api.Tests.Controllers;

public class CategoriesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CategoriesController _controller;

    public CategoriesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new CategoriesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnOkResult_WithCategoriesList()
    {
        // Arrange
        var categories = new List<CategoryDto>
        {
            new() { Id = Guid.NewGuid(), Name = "Lanche", ImageUrl = "lanche.png" },
            new() { Id = Guid.NewGuid(), Name = "Bebida", ImageUrl = "bebida.png" }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<ListCategoriesQuery>(), default))
                     .ReturnsAsync(categories);

        // Act
        var result = await _controller.ListAsync();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(categories);
    }

    [Fact]
    public async Task GetAsync_WithValidId_ShouldReturnOkResult()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var category = new CategoryDto { Id = categoryId, Name = "Lanche", ImageUrl = "lanche.png" };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCategoryQuery>(), default))
                     .ReturnsAsync(category);

        // Act
        var result = await _controller.GetAsync(categoryId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(category);
    }

    [Fact]
    public async Task GetAsync_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCategoryQuery>(), default))
                     .ReturnsAsync((CategoryDto?)null);

        // Act
        var result = await _controller.GetAsync(categoryId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ShouldReturnOkResult()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("lanche.png");
        mockFile.Setup(f => f.ContentType).Returns("image/png");
        mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());
        
        var request = new CreateCategoryRequest("Lanche", mockFile.Object);
        var categoryDto = new CategoryDto { Id = Guid.NewGuid(), Name = "Lanche", ImageUrl = "lanche.png" };

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCategoryCommand>(), default))
                     .ReturnsAsync(categoryDto);

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(categoryDto);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteCategoryCommand>(), default))
                     .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.DeleteAsync(categoryId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task CreateAsync_ShouldCallMediatorWithCorrectCommand()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("lanche.png");
        mockFile.Setup(f => f.ContentType).Returns("image/png");
        mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());
        
        var request = new CreateCategoryRequest("Lanche", mockFile.Object);
        var categoryDto = new CategoryDto { Id = Guid.NewGuid(), Name = "Lanche", ImageUrl = "lanche.png" };

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCategoryCommand>(), default))
                     .ReturnsAsync(categoryDto);

        // Act
        await _controller.CreateAsync(request);

        // Assert
        _mediatorMock.Verify(m => m.Send(
            It.Is<CreateCategoryCommand>(cmd => cmd.Name == request.Name), 
            default), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallMediatorWithCorrectCommand()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        // Act
        await _controller.DeleteAsync(categoryId);

        // Assert
        _mediatorMock.Verify(m => m.Send(
            It.Is<DeleteCategoryCommand>(cmd => cmd.Id == categoryId), 
            default), Times.Once);
    }
}