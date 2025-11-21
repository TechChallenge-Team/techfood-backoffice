using FluentAssertions;
using NSubstitute;
using TechFood.BackOffice.Application.Common.Services.Interfaces;

namespace TechFood.BackOffice.Infra.Tests.Services;

public class ImageStorageServiceTests
{
    private readonly IImageStorageService _mockImageStorageService;
    private readonly string _testFileName = "test-image.jpg";
    private readonly string _testFolder = "testfolder";

    public ImageStorageServiceTests()
    {
        _mockImageStorageService = Substitute.For<IImageStorageService>();
    }

    [Fact]
    public async Task SaveAsync_ShouldCallServiceWithCorrectParameters()
    {
        // Arrange
        var imageContent = "This is a test image content"u8.ToArray();
        using var imageStream = new MemoryStream(imageContent);

        // Act
        await _mockImageStorageService.SaveAsync(imageStream, _testFileName, _testFolder);

        // Assert
        await _mockImageStorageService.Received(1).SaveAsync(
            Arg.Any<Stream>(), 
            _testFileName, 
            _testFolder);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallServiceWithCorrectParameters()
    {
        // Act
        await _mockImageStorageService.DeleteAsync(_testFileName, _testFolder);

        // Assert
        await _mockImageStorageService.Received(1).DeleteAsync(_testFileName, _testFolder);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingFile_ShouldNotThrow()
    {
        // Arrange
        var nonExistingFileName = "non-existing-file.jpg";

        // Act & Assert
        var act = async () => await _mockImageStorageService.DeleteAsync(nonExistingFileName, _testFolder);
        await act.Should().NotThrowAsync();

        await _mockImageStorageService.Received(1).DeleteAsync(nonExistingFileName, _testFolder);
    }

    [Fact]
    public async Task SaveAsync_WithEmptyStream_ShouldCallService()
    {
        // Arrange
        using var emptyStream = new MemoryStream();

        // Act
        await _mockImageStorageService.SaveAsync(emptyStream, _testFileName, _testFolder);

        // Assert
        await _mockImageStorageService.Received(1).SaveAsync(
            Arg.Any<Stream>(),
            _testFileName,
            _testFolder);
    }
}