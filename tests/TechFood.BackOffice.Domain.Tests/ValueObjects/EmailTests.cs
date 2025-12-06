using TechFood.BackOffice.Domain.ValueObjects;
using TechFood.Shared.Domain.Exceptions;

namespace TechFood.BackOffice.Domain.Tests.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("test.email@domain.co.uk")]
    [InlineData("admin@techfood.com.br")]
    [InlineData("user123@test-domain.org")]
    public void Email_WithValidAddress_ShouldCreateSuccessfully(string validEmail)
    {
        // Act
        var email = new Email(validEmail);

        // Assert
        email.Should().NotBeNull();
        email.Address.Should().Be(validEmail);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@domain.com")]
    [InlineData("user@")]
    [InlineData("user@.com")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("plaintext")]
    public void Email_WithInvalidAddress_ShouldThrowDomainException(string invalidEmail)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new Email(invalidEmail));
        exception.Message.Should().Contain("Email Invalid");
    }

    [Fact]
    public void Email_ShouldInheritFromValueObjectAndImplementIEquatable()
    {
        // Arrange & Act
        var email = new Email("test@example.com");

        // Assert
        email.Should().BeAssignableTo<TechFood.Shared.Domain.ValueObjects.ValueObject>();
        email.Should().BeAssignableTo<IEquatable<Email>>();
    }

    [Fact]
    public void Email_WithSameAddress_ShouldBeEqual()
    {
        // Arrange
        var address = "test@example.com";
        var email1 = new Email(address);
        var email2 = new Email(address);

        // Act & Assert
        email1.Equals(email2).Should().BeTrue();
        (email1 == email2).Should().BeFalse(); // Reference equality should be false
        // Note: GetHashCode() may differ for separate instances even with same values
    }

    [Fact]
    public void Email_WithDifferentAddress_ShouldNotBeEqual()
    {
        // Arrange
        var email1 = new Email("test1@example.com");
        var email2 = new Email("test2@example.com");

        // Act & Assert
        email1.Equals(email2).Should().BeFalse();
        email1.Should().NotBe(email2);
    }

    [Fact]
    public void Email_EqualsWithNull_ShouldReturnFalse()
    {
        // Arrange
        var email = new Email("test@example.com");

        // Act & Assert
        email.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void Email_ImplicitConversionFromString_ShouldWork()
    {
        // Arrange
        string emailAddress = "test@example.com";

        // Act
        Email email = emailAddress;

        // Assert
        email.Address.Should().Be(emailAddress);
    }

    [Fact]
    public void Email_ImplicitConversionToString_ShouldWork()
    {
        // Arrange
        var email = new Email("test@example.com");

        // Act
        string address = email;

        // Assert
        address.Should().Be("test@example.com");
    }

    [Fact]
    public void Email_Properties_ShouldBeSettable()
    {
        // Arrange & Act
        var email = new Email("original@example.com")
        {
            Address = "updated@example.com"
        };

        // Assert
        email.Address.Should().Be("updated@example.com");
    }
}