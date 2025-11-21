using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Application.Common.Services;

namespace TechFood.BackOffice.Application.Tests.Services;

public class ImageUrlResolverTests
{
    private readonly IImageUrlResolver _imageUrlResolver;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly string _baseUrl = "https://api.techfood.com";

    public ImageUrlResolverTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        
        _configurationMock.Setup(c => c["TechFoodStaticImagesUrl"])
                          .Returns("images");
        
        // Setup HttpContext mock
        var httpContextMock = new Mock<HttpContext>();
        var requestMock = new Mock<HttpRequest>();
        
        requestMock.Setup(r => r.Scheme).Returns("https");
        requestMock.Setup(r => r.Host).Returns(new HostString("api.techfood.com"));
        requestMock.Setup(r => r.PathBase).Returns(new PathString(""));
        
        httpContextMock.Setup(c => c.Request).Returns(requestMock.Object);
        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContextMock.Object);
        
        _imageUrlResolver = new ImageUrlResolver(_configurationMock.Object, _httpContextAccessorMock.Object);
    }

    [Fact]
    public void BuildFilePath_WithValidParameters_ShouldReturnCorrectUrl()
    {
        // Arrange
        var folder = "category";
        var fileName = "lanche.png";

        // Act
        var result = _imageUrlResolver.BuildFilePath(folder, fileName);

        // Assert
        result.Should().Contain("api.techfood.com");
        result.Should().Contain("category");
        result.Should().Contain("lanche.png");
    }

    [Theory]
    [InlineData("category", "lanche.png")]
    [InlineData("product", "burger.png")]
    [InlineData("test", "image.jpg")]
    public void BuildFilePath_WithDifferentInputs_ShouldReturnExpectedUrls(string folder, string fileName)
    {
        // Act
        var result = _imageUrlResolver.BuildFilePath(folder, fileName);

        // Assert
        result.Should().Contain("api.techfood.com");
        result.Should().Contain(folder);
        result.Should().Contain(fileName);
    }

    [Fact]
    public void BuildFilePath_WithEmptyFolder_ShouldThrowArgumentException()
    {
        // Arrange
        var folder = "";
        var fileName = "lanche.png";

        // Act & Assert
        Assert.Throws<ApplicationException>(() => _imageUrlResolver.BuildFilePath(folder, fileName));
    }

    [Fact]
    public void BuildFilePath_WithNullFolder_ShouldThrowArgumentException()
    {
        // Arrange
        string? folder = null;
        var fileName = "lanche.png";

        // Act & Assert
        Assert.Throws<ApplicationException>(() => _imageUrlResolver.BuildFilePath(folder!, fileName));
    }

    [Fact]
    public void BuildFilePath_WithEmptyFileName_ShouldThrowArgumentException()
    {
        // Arrange
        var folder = "category";
        var fileName = "";

        // Act & Assert
        Assert.Throws<ApplicationException>(() => _imageUrlResolver.BuildFilePath(folder, fileName));
    }

    [Fact]
    public void BuildFilePath_WithNullFileName_ShouldThrowArgumentException()
    {
        // Arrange
        var folder = "category";
        string? fileName = null;

        // Act & Assert
        Assert.Throws<ApplicationException>(() => _imageUrlResolver.BuildFilePath(folder, fileName!));
    }

    [Fact]
    public void BuildFilePath_WithBaseUrlEndingInSlash_ShouldHandleCorrectly()
    {
        // Arrange
        var configMock = new Mock<IConfiguration>();
        var httpContextMock = new Mock<IHttpContextAccessor>();
        var httpContextMockObject = new Mock<HttpContext>();
        var requestMock = new Mock<HttpRequest>();
        
        configMock.Setup(c => c["TechFoodStaticImagesUrl"]).Returns("https://api.techfood.com/");
        requestMock.SetupGet(r => r.PathBase).Returns(new PathString(""));
        httpContextMockObject.SetupGet(c => c.Request).Returns(requestMock.Object);
        httpContextMock.SetupGet(h => h.HttpContext).Returns(httpContextMockObject.Object);
        
        var resolver = new ImageUrlResolver(configMock.Object, httpContextMock.Object);
        var folder = "category";
        var fileName = "lanche.png";

        // Act
        var result = resolver.BuildFilePath(folder, fileName);

        // Assert
        result.Should().Be("/https://api.techfood.com/category/lanche.png");
    }

    [Theory]
    [InlineData("CATEGORY", "Lanche.PNG")]
    [InlineData("Category", "lanche.Png")]
    public void BuildFilePath_ShouldPreserveCasing(string folder, string fileName)
    {
        // Act
        var result = _imageUrlResolver.BuildFilePath(folder, fileName);

        // Assert
        result.Should().Contain(folder);
        result.Should().Contain(fileName);
    }

    [Fact]
    public void BuildFilePath_WithSpecialCharactersInFileName_ShouldHandleCorrectly()
    {
        // Arrange
        var folder = "category";
        var fileName = "x-burger_special-01.png";

        // Act
        var result = _imageUrlResolver.BuildFilePath(folder, fileName);

        // Assert
        result.Should().Be($"{_baseUrl}/images/{folder}/{fileName}");
    }
}