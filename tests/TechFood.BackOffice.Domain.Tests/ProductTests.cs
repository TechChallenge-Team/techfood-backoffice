using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Tests.Fixtures;

namespace TechFood.BackOffice.Domain.Tests;

public class ProductTests
{
    [Fact]
    public void Product_WithValidParameters_ShouldCreateSuccessfully()
    {
        // Arrange & Act
        var product = ProductFixture.CreateValid();

        // Assert
        product.Should().NotBeNull();
        product.Name.Should().Be("X-Burguer");
        product.Description.Should().Be("Delicioso hambúrguer");
        product.CategoryId.Should().NotBe(Guid.Empty);
        product.ImageFileName.Should().Be("burger.png");
        product.Price.Should().Be(19.99m);
        product.OutOfStock.Should().BeFalse();
    }

    [Theory]
    [InlineData("", "Description", "burger.png", 19.99)]
    [InlineData(null, "Description", "burger.png", 19.99)]
    [InlineData("  ", "Description", "burger.png", 19.99)]
    public void Product_WithInvalidName_ShouldThrowArgumentException(string name, string description, string imageFileName, decimal price)
    {
        // Act & Assert
        Assert.Throws<TechFood.Shared.Domain.Exceptions.DomainException>(() => new Product(name, description, Guid.NewGuid(), imageFileName, price));
    }

    [Theory]
    [InlineData("Product", "", "burger.png", 19.99)]
    [InlineData("Product", null, "burger.png", 19.99)]
    [InlineData("Product", "  ", "burger.png", 19.99)]
    public void Product_WithInvalidDescription_ShouldThrowArgumentException(string name, string description, string imageFileName, decimal price)
    {
        // Act & Assert
        Assert.Throws<TechFood.Shared.Domain.Exceptions.DomainException>(() => new Product(name, description, Guid.NewGuid(), imageFileName, price));
    }

    [Theory]
    [InlineData("Product", "Description", "", 19.99)]
    [InlineData("Product", "Description", null, 19.99)]
    [InlineData("Product", "Description", "  ", 19.99)]
    public void Product_WithInvalidImageFileName_ShouldThrowArgumentException(string name, string description, string imageFileName, decimal price)
    {
        // Act & Assert
        Assert.Throws<TechFood.Shared.Domain.Exceptions.DomainException>(() => new Product(name, description, Guid.NewGuid(), imageFileName, price));
    }



    [Fact]
    public void Product_WithEmptyCategoryId_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<TechFood.Shared.Domain.Exceptions.DomainException>(() => new Product("Product", "Description", Guid.Empty, "image.png", 19.99m));
    }

    [Fact]
    public void Product_ShouldHaveUniqueId()
    {
        // Arrange & Act
        var product1 = ProductFixture.CreateValid();
        var product2 = ProductFixture.CreateValid();

        // Assert
        product1.Id.Should().NotBe(product2.Id);
        product1.Id.Should().NotBe(Guid.Empty);
        product2.Id.Should().NotBe(Guid.Empty);
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(1.00)]
    [InlineData(99.99)]
    [InlineData(999.99)]
    public void Product_WithValidPrice_ShouldSetCorrectly(decimal price)
    {
        // Act
        var product = ProductFixture.CreateWithPrice(price);

        // Assert
        product.Price.Should().Be(price);
    }







    [Fact]
    public void SetOutOfStock_ShouldMarkProductAsOutOfStock()
    {
        // Arrange
        var product = ProductFixture.CreateValid();

        // Act
        product.SetOutOfStock(true);

        // Assert
        product.OutOfStock.Should().BeTrue();
    }

    [Fact]
    public void SetInStock_ShouldMarkProductAsInStock()
    {
        // Arrange
        var product = ProductFixture.CreateOutOfStock();

        // Act
        product.SetOutOfStock(false);

        // Assert
        product.OutOfStock.Should().BeFalse();
    }

    [Fact]
    public void Product_ByDefault_ShouldBeInStock()
    {
        // Arrange & Act
        var product = ProductFixture.CreateValid();

        // Assert
        product.OutOfStock.Should().BeFalse();
    }

    [Fact]
    public void Product_WithLongName_ShouldAccept()
    {
        // Arrange
        var longName = new string('A', 200);

        // Act
        var product = new Product(longName, "Description", Guid.NewGuid(), "image.png", 19.99m);

        // Assert
        product.Name.Should().Be(longName);
    }

    [Fact]
    public void Product_WithSpecialCharacters_ShouldAccept()
    {
        // Arrange
        var nameWithSpecialChars = "X-Burguer & Café";
        var descriptionWithSpecialChars = "Delicioso hambúrguer com café";

        // Act
        var product = new Product(nameWithSpecialChars, descriptionWithSpecialChars, Guid.NewGuid(), "burger.png", 19.99m);

        // Assert
        product.Name.Should().Be(nameWithSpecialChars);
        product.Description.Should().Be(descriptionWithSpecialChars);
    }
}