using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Tests.Fixtures;

namespace TechFood.BackOffice.Domain.Tests;

public class CategoryTests
{
    [Fact]
    public void Category_WithValidParameters_ShouldCreateSuccessfully()
    {
        // Arrange & Act
        var category = CategoryFixture.CreateValid();

        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be("Lanche");
        category.ImageFileName.Should().Be("lanche.png");
        category.SortOrder.Should().Be(0);
    }

    [Theory]
    [InlineData("", "lanche.png", 0)]
    [InlineData(null, "lanche.png", 0)]
    [InlineData("  ", "lanche.png", 0)]
    public void Category_WithInvalidName_ShouldThrowArgumentException(string name, string imageFileName, int sortOrder)
    {
        // Act & Assert
        Assert.Throws<TechFood.Shared.Domain.Exceptions.DomainException>(() => new Category(name, imageFileName, sortOrder));
    }

    [Theory]
    [InlineData("Lanche", "", 0)]
    [InlineData("Lanche", null, 0)]
    [InlineData("Lanche", "  ", 0)]
    public void Category_WithInvalidImageFileName_ShouldThrowArgumentException(string name, string imageFileName, int sortOrder)
    {
        // Act & Assert
        Assert.Throws<TechFood.Shared.Domain.Exceptions.DomainException>(() => new Category(name, imageFileName, sortOrder));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Category_WithNegativeSortOrder_ShouldThrowArgumentException(int sortOrder)
    {
        // Act & Assert
        Assert.Throws<TechFood.Shared.Domain.Exceptions.DomainException>(() => new Category("Lanche", "lanche.png", sortOrder));
    }

    [Fact]
    public void Category_ShouldHaveUniqueId()
    {
        // Arrange & Act
        var category1 = CategoryFixture.CreateValid();
        var category2 = CategoryFixture.CreateValid();

        // Assert
        category1.Id.Should().NotBe(category2.Id);
        category1.Id.Should().NotBe(Guid.Empty);
        category2.Id.Should().NotBe(Guid.Empty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(99)]
    [InlineData(1000)]
    public void Category_WithValidSortOrder_ShouldSetCorrectly(int sortOrder)
    {
        // Act
        var category = new Category("Test Category", "test.png", sortOrder);

        // Assert
        category.SortOrder.Should().Be(sortOrder);
    }





    [Fact]
    public void Category_WithLongName_ShouldAccept()
    {
        // Arrange
        var longName = new string('A', 100);

        // Act
        var category = new Category(longName, "image.png", 0);

        // Assert
        category.Name.Should().Be(longName);
    }

    [Fact]
    public void Category_WithSpecialCharacters_ShouldAccept()
    {
        // Arrange
        var nameWithSpecialChars = "Café & Açaí";
        var imageWithSpecialChars = "café_açaí.png";

        // Act
        var category = new Category(nameWithSpecialChars, imageWithSpecialChars, 0);

        // Assert
        category.Name.Should().Be(nameWithSpecialChars);
        category.ImageFileName.Should().Be(imageWithSpecialChars);
    }

    [Fact]
    public void Categories_WithSameSortOrder_ShouldBeAllowed()
    {
        // Arrange & Act
        var category1 = new Category("Category 1", "cat1.png", 1);
        var category2 = new Category("Category 2", "cat2.png", 1);

        // Assert
        category1.SortOrder.Should().Be(category2.SortOrder);
    }
}