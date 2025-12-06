using TechFood.BackOffice.Application.Products.Commands.CreateProduct;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Repositories;

namespace TechFood.BackOffice.Application.Tests.Commands;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IImageUrlResolver> _imageUrlResolverMock;
    private readonly Mock<IImageStorageService> _imageStorageServiceMock;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _imageUrlResolverMock = new Mock<IImageUrlResolver>();
        _imageStorageServiceMock = new Mock<IImageStorageService>();
        
        _handler = new CreateProductCommandHandler(
            _productRepositoryMock.Object,
            _categoryRepositoryMock.Object,
            _imageUrlResolverMock.Object,
            _imageStorageServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateProductAndReturnDto()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var stream = new MemoryStream("fake image content"u8.ToArray());
        var command = new CreateProductCommand("X-Burguer", "Delicioso hambúrguer", categoryId, stream, "image/png", 19.99m);
        var existingCategory = new Category("Lanche", "lanche.png", 0);

        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(categoryId))
                               .ReturnsAsync(existingCategory);
        
        _productRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
                              .ReturnsAsync(Guid.NewGuid());
        
        _imageUrlResolverMock.Setup(x => x.CreateImageFileName(It.IsAny<string>(), It.IsAny<string>()))
                            .Returns("burger.png");
        
        _imageUrlResolverMock.Setup(x => x.BuildFilePath(It.IsAny<string>(), It.IsAny<string>()))
                            .Returns("/images/products/burger.png");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("X-Burguer");
        result.Description.Should().Be("Delicioso hambúrguer");
        result.Price.Should().Be(19.99m);
        _productRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        _imageStorageServiceMock.Verify(s => s.SaveAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CategoryNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var stream = new MemoryStream("fake image content"u8.ToArray());
        var command = new CreateProductCommand("X-Burguer", "Delicioso hambúrguer", categoryId, stream, "image/png", 19.99m);

        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(categoryId))
                               .ReturnsAsync((Category?)null);

        // Act & Assert
        await Assert.ThrowsAsync<TechFood.Shared.Application.Exceptions.NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Theory]
    [InlineData("", "Description", 19.99)]
    [InlineData(null, "Description", 19.99)]
    [InlineData("Name", "", 19.99)]
    [InlineData("Name", null, 19.99)]
    public async Task Handle_InvalidCommand_ShouldThrowArgumentException(string name, string description, decimal price)
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var stream = new MemoryStream("fake image content"u8.ToArray());
        var command = new CreateProductCommand(name, description, categoryId, stream, "image/png", price);
        var existingCategory = new Category("Lanche", "lanche.png", 0);

        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(categoryId))
                               .ReturnsAsync(existingCategory);
        
        _imageUrlResolverMock.Setup(x => x.CreateImageFileName(It.IsAny<string>(), It.IsAny<string>()))
                            .Returns("product.png");

        // Act & Assert
        await Assert.ThrowsAsync<TechFood.Shared.Domain.Exceptions.DomainException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldVerifyCategoryExists()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var stream = new MemoryStream("fake image content"u8.ToArray());
        var command = new CreateProductCommand("X-Burguer", "Delicioso hambúrguer", categoryId, stream, "image/png", 19.99m);
        var existingCategory = new Category("Lanche", "lanche.png", 0);

        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(categoryId))
                               .ReturnsAsync(existingCategory);
        
        _productRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
                              .ReturnsAsync(Guid.NewGuid());
        
        _imageUrlResolverMock.Setup(x => x.CreateImageFileName(It.IsAny<string>(), It.IsAny<string>()))
                            .Returns("burger.png");
        
        _imageUrlResolverMock.Setup(x => x.BuildFilePath(It.IsAny<string>(), It.IsAny<string>()))
                            .Returns("/images/products/burger.png");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _categoryRepositoryMock.Verify(r => r.GetByIdAsync(categoryId), Times.Once);
    }
}