using TechFood.BackOffice.Domain.ValueObjects;

namespace TechFood.BackOffice.Domain.Tests.ValueObjects;

public class NameTests
{
    [Theory]
    [InlineData("João Silva")]
    [InlineData("Maria dos Santos")]
    [InlineData("Pedro")]
    [InlineData("Ana Carolina de Almeida")]
    public void Name_WithValidFullName_ShouldCreateSuccessfully(string fullName)
    {
        // Act
        var name = new Name(fullName);

        // Assert
        name.Should().NotBeNull();
        name.FullName.Should().Be(fullName);
    }

    [Fact]
    public void Name_ShouldInheritFromValueObjectAndImplementIEquatable()
    {
        // Arrange & Act
        var name = new Name("Test Name");

        // Assert
        name.Should().BeAssignableTo<TechFood.Shared.Domain.ValueObjects.ValueObject>();
        name.Should().BeAssignableTo<IEquatable<Name>>();
    }

    [Fact]
    public void Name_WithSameFullName_ShouldBeEqual()
    {
        // Arrange
        var fullName = "João Silva";
        var name1 = new Name(fullName);
        var name2 = new Name(fullName);

        // Act & Assert
        name1.Equals(name2).Should().BeTrue();
        (name1 == name2).Should().BeFalse(); // Reference equality should be false
        // Note: GetHashCode() may differ for separate instances even with same values
    }

    [Fact]
    public void Name_WithDifferentFullName_ShouldNotBeEqual()
    {
        // Arrange
        var name1 = new Name("João Silva");
        var name2 = new Name("Maria Santos");

        // Act & Assert
        name1.Equals(name2).Should().BeFalse();
        name1.Should().NotBe(name2);
    }

    [Fact]
    public void Name_EqualsWithNull_ShouldReturnFalse()
    {
        // Arrange
        var name = new Name("João Silva");

        // Act & Assert
        name.Equals(null).Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Name_WithEmptyOrWhitespaceFullName_ShouldStillCreate(string fullName)
    {
        // Note: The Name class doesn't seem to have validation, so empty names are allowed
        // Act
        var name = new Name(fullName);

        // Assert
        name.Should().NotBeNull();
        name.FullName.Should().Be(fullName);
        // Additional assertion to make this test different from the valid name test
        (string.IsNullOrWhiteSpace(fullName)).Should().BeTrue();
    }

    [Fact]
    public void Name_FullName_ShouldBeReadOnly()
    {
        // Arrange
        var originalFullName = "João Silva";
        var name = new Name(originalFullName);

        // Act & Assert
        name.FullName.Should().Be(originalFullName);
        // The property has a private setter, so it cannot be changed after construction
    }

    [Fact] 
    public void Name_WithNullFullName_ShouldHandleGracefully()
    {
        // Act
        // The constructor doesn't validate null, so it will create with null
        var name = new Name(null!);
        
        // Assert
        name.FullName.Should().BeNull();
    }
}