using TechFood.BackOffice.Application.Categories.Commands.CreateCategory;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Repositories;

namespace TechFood.BackOffice.Application.Tests.Commands;

public class CreateCategoryCommandHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IImageStorageService> _imageStorageServiceMock;
    private readonly Mock<IImageUrlResolver> _imageUrlResolverMock;
    private readonly CreateCategoryCommandHandler _handler;

    public CreateCategoryCommandHandlerTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _imageStorageServiceMock = new Mock<IImageStorageService>();
        _imageUrlResolverMock = new Mock<IImageUrlResolver>();
        
        _handler = new CreateCategoryCommandHandler(
            _categoryRepositoryMock.Object,
            _imageStorageServiceMock.Object,
            _imageUrlResolverMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateCategoryAndReturnDto()
    {
        // Arrange
        var stream = new MemoryStream("fake image content"u8.ToArray());
        var command = new CreateCategoryCommand("Lanche", stream, "image/png");

        _categoryRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Category>()))
                               .ReturnsAsync(Guid.NewGuid());
        
        _imageUrlResolverMock.Setup(x => x.CreateImageFileName(It.IsAny<string>(), It.IsAny<string>()))
                            .Returns("lanche.png");
        
        _imageUrlResolverMock.Setup(x => x.BuildFilePath(It.IsAny<string>(), It.IsAny<string>()))
                            .Returns("/images/categories/lanche.png");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Lanche");
        result.ImageUrl.Should().Be("/images/categories/lanche.png");
        _categoryRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
        _imageStorageServiceMock.Verify(s => s.SaveAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Handle_InvalidName_ShouldThrowArgumentException(string name)
    {
        // Arrange
        var stream = new MemoryStream("fake image content"u8.ToArray());
        var command = new CreateCategoryCommand(name, stream, "image/png");

        // Act & Assert
        await Assert.ThrowsAsync<TechFood.Shared.Domain.Exceptions.DomainException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var stream = new MemoryStream("fake image content"u8.ToArray());
        var command = new CreateCategoryCommand("Lanche", stream, "image/png");
        var expectedException = new Exception("Database error");

        _imageUrlResolverMock.Setup(x => x.CreateImageFileName(It.IsAny<string>(), It.IsAny<string>()))
                            .Returns("lanche.png");
                            
        _categoryRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Category>()))
                               .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        exception.Should().Be(expectedException);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryOnce()
    {
        // Arrange
        var stream = new MemoryStream("fake image content"u8.ToArray());
        var command = new CreateCategoryCommand("Lanche", stream, "image/png");

        _categoryRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Category>()))
                               .ReturnsAsync(Guid.NewGuid());
        
        _imageUrlResolverMock.Setup(x => x.CreateImageFileName(It.IsAny<string>(), It.IsAny<string>()))
                            .Returns("lanche.png");
        
        _imageUrlResolverMock.Setup(x => x.BuildFilePath(It.IsAny<string>(), It.IsAny<string>()))
                            .Returns("/images/categories/lanche.png");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _categoryRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
    }
}