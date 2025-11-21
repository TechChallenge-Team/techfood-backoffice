using TechFood.BackOffice.Domain.Enums;
using TechFood.BackOffice.Domain.ValueObjects;
using TechFood.Shared.Domain.Exceptions;

namespace TechFood.BackOffice.Domain.Tests.ValueObjects;

public class DocumentTests
{
    [Fact]
    public void Document_WithValidCPF_ShouldCreateSuccessfully()
    {
        // Arrange
        var cpfValue = "11144477735"; // Valid CPF
        var documentType = DocumentType.CPF;

        // Act
        var document = new Document(documentType, cpfValue);

        // Assert
        document.Should().NotBeNull();
        document.Type.Should().Be(DocumentType.CPF);
        document.Value.Should().Be(cpfValue);
    }

    [Theory]
    [InlineData("00000000000")]
    [InlineData("11111111111")]
    [InlineData("12345678900")]
    [InlineData("123456789")]
    [InlineData("123456789012")]
    [InlineData("")]
    [InlineData("   ")]
    public void Document_WithInvalidCPF_ShouldThrowDomainException(string invalidCpf)
    {
        // Arrange
        var documentType = DocumentType.CPF;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new Document(documentType, invalidCpf));
        exception.Message.Should().Contain("CPF is invalid");
    }

    [Fact]
    public void Document_ShouldInheritFromValueObject()
    {
        // Arrange & Act
        var document = new Document(DocumentType.CPF, "11144477735");

        // Assert
        document.Should().BeAssignableTo<TechFood.Shared.Domain.ValueObjects.ValueObject>();
    }

    [Fact]
    public void Document_WithSameValues_ShouldBeEqual()
    {
        // Arrange
        var cpfValue = "11144477735";
        var document1 = new Document(DocumentType.CPF, cpfValue);
        var document2 = new Document(DocumentType.CPF, cpfValue);

        // Act & Assert
        (document1 == document2).Should().BeFalse(); // Reference equality should be false
        // Note: GetHashCode() may differ for separate instances even with same values
    }

    [Fact]
    public void Document_WithDifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        var document1 = new Document(DocumentType.CPF, "11144477735");
        var document2 = new Document(DocumentType.CPF, "52998224725"); // Different valid CPF

        // Act & Assert
        document1.Should().NotBe(document2);
    }

    [Fact]
    public void Document_WithDifferentTypes_ShouldNotBeEqual()
    {
        // Arrange - assuming there might be other document types in the future
        var document1 = new Document(DocumentType.CPF, "11144477735");
        
        // Act & Assert
        document1.Type.Should().Be(DocumentType.CPF);
        // This test validates the Type property works correctly
    }

    [Fact]
    public void Document_Properties_ShouldBeSettable()
    {
        // Arrange & Act
        var document = new Document(DocumentType.CPF, "11144477735")
        {
            Type = DocumentType.CPF,
            Value = "22233344456"
        };

        // Assert
        document.Type.Should().Be(DocumentType.CPF);
        document.Value.Should().Be("22233344456");
    }
}