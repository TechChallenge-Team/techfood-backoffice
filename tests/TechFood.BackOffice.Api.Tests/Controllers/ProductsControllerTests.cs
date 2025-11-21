using MediatR;
using TechFood.Api.Controllers;
using TechFood.BackOffice.Application.Products.Commands.CreateProduct;
using TechFood.BackOffice.Application.Products.Commands.DeleteProduct;
using TechFood.BackOffice.Application.Products.Commands.UpdateProduct;
using TechFood.BackOffice.Application.Products.Dto;
using TechFood.BackOffice.Application.Products.Queries.ListProducts;
using TechFood.BackOffice.Application.Products.Queries.GetProduct;
using TechFood.BackOffice.Contracts.Products;
using Microsoft.AspNetCore.Http;

namespace TechFood.BackOffice.Api.Tests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ProductsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOkResult_WithProductsList()
    {
        // Arrange
        var products = new List<ProductDto>
        {
            new(Guid.NewGuid(), "X-Burguer", "Delicioso hambúrguer", Guid.NewGuid(), false, "burger.png", 19.99m),
            new(Guid.NewGuid(), "Batata Frita", "Batatas crocantes", Guid.NewGuid(), false, "fries.png", 9.99m)
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<ListProductsQuery>(), default))
                     .ReturnsAsync(products);

        // Act
        var result = await _controller.GetAllAsync();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnOkResult()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new ProductDto(productId, "X-Burguer", "Delicioso hambúrguer", Guid.NewGuid(), false, "burger.png", 19.99m);

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductQuery>(), default))
                     .ReturnsAsync(product);

        // Act
        var result = await _controller.GetByIdAsync(productId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductQuery>(), default))
                     .ReturnsAsync((ProductDto?)null);

        // Act
        var result = await _controller.GetByIdAsync(productId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ShouldReturnOkResult()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("burger.png");
        mockFile.Setup(f => f.ContentType).Returns("image/png");
        mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());
        
        var request = new CreateUpdateRequest("X-Burguer", "Delicioso hambúrguer", Guid.NewGuid(), 19.99m, mockFile.Object);
        var productDto = new ProductDto(Guid.NewGuid(), "X-Burguer", "Delicioso hambúrguer", Guid.NewGuid(), false, "burger.png", 19.99m);

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), default))
                     .ReturnsAsync(productDto);

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(productDto);
    }

    [Fact]
    public async Task UpdateAsync_WithValidRequest_ShouldReturnOk()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("burger.png");
        mockFile.Setup(f => f.ContentType).Returns("image/png");
        mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());
        
        var request = new UpdateProductRequest("X-Burguer Updated", "Descrição atualizada", Guid.NewGuid(), 21.99m, mockFile.Object);
        var productDto = new ProductDto(productId, "X-Burguer Updated", "Descrição atualizada", Guid.NewGuid(), false, "burger.png", 21.99m);

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), default))
                     .ReturnsAsync(productDto);

        // Act
        var result = await _controller.UpdateAsync(productId, request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(productDto);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), default))
                     .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.DeleteAsync(productId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task CreateAsync_ShouldCallMediatorWithCorrectCommand_V2()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("burger.png");
        mockFile.Setup(f => f.ContentType).Returns("image/png");
        mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());
        
        var request = new CreateUpdateRequest("X-Burguer", "Delicioso hambúrguer", Guid.NewGuid(), 19.99m, mockFile.Object);
        var productDto = new ProductDto(Guid.NewGuid(), "X-Burguer", "Delicioso hambúrguer", Guid.NewGuid(), false, "burger.png", 19.99m);

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), default))
                     .ReturnsAsync(productDto);

        // Act
        await _controller.CreateAsync(request);

        // Assert
        _mediatorMock.Verify(m => m.Send(
            It.Is<CreateProductCommand>(cmd => 
                cmd.Name == request.Name && 
                cmd.Description == request.Description && 
                cmd.CategoryId == request.CategoryId &&
                cmd.Price == request.Price), 
            default), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldCallMediatorWithCorrectCommand()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("burger.png");
        mockFile.Setup(f => f.ContentType).Returns("image/png");
        mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());
        
        var request = new UpdateProductRequest("X-Burguer Updated", "Descrição atualizada", Guid.NewGuid(), 21.99m, mockFile.Object);
        var productDto = new ProductDto(productId, "X-Burguer Updated", "Descrição atualizada", Guid.NewGuid(), false, "burger.png", 21.99m);

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), default))
                     .ReturnsAsync(productDto);

        // Act
        await _controller.UpdateAsync(productId, request);

        // Assert
        _mediatorMock.Verify(m => m.Send(
            It.Is<UpdateProductCommand>(cmd => 
                cmd.Id == productId &&
                cmd.Name == request.Name && 
                cmd.Description == request.Description && 
                cmd.CategoryId == request.CategoryId &&
                cmd.Price == request.Price), 
            default), Times.Once);
    }
}