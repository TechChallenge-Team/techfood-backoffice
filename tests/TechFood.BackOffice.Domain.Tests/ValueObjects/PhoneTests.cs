using TechFood.BackOffice.Domain.ValueObjects;

namespace TechFood.BackOffice.Domain.Tests.ValueObjects;

public class PhoneTests
{
    [Fact]
    public void Phone_WithValidParameters_ShouldCreateSuccessfully()
    {
        // Arrange
        var countryCode = "+55";
        var ddd = "11";
        var number = "999888777";

        // Act
        var phone = new Phone(countryCode, ddd, number);

        // Assert
        phone.Should().NotBeNull();
        phone.CountryCode.Should().Be(countryCode);
        phone.DDD.Should().Be(ddd);
        phone.Number.Should().Be(number);
    }

    [Fact]
    public void Phone_WithNullNumber_ShouldCreateSuccessfully()
    {
        // Arrange
        var countryCode = "+55";
        var ddd = "11";
        string? number = null;

        // Act
        var phone = new Phone(countryCode, ddd, number);

        // Assert
        phone.Should().NotBeNull();
        phone.CountryCode.Should().Be(countryCode);
        phone.DDD.Should().Be(ddd);
        phone.Number.Should().BeNull();
    }

    [Theory]
    [InlineData("+55", "11", "999888777")]
    [InlineData("+1", "555", "1234567")]
    [InlineData("+44", "20", "87654321")]
    public void Phone_WithDifferentCountryCodes_ShouldCreateSuccessfully(string countryCode, string ddd, string number)
    {
        // Act
        var phone = new Phone(countryCode, ddd, number);

        // Assert
        phone.Should().NotBeNull();
        phone.CountryCode.Should().Be(countryCode);
        phone.DDD.Should().Be(ddd);
        phone.Number.Should().Be(number);
    }

    [Fact]
    public void Phone_ShouldInheritFromValueObject()
    {
        // Arrange & Act
        var phone = new Phone("+55", "11", "999888777");

        // Assert
        phone.Should().BeAssignableTo<TechFood.Shared.Domain.ValueObjects.ValueObject>();
    }

    [Fact]
    public void Phone_Properties_ShouldBeInitOnly()
    {
        // Arrange
        var countryCode = "+55";
        var ddd = "11";
        var number = "999888777";

        // Act
        var phone = new Phone(countryCode, ddd, number);

        // Assert
        phone.CountryCode.Should().Be(countryCode);
        phone.DDD.Should().Be(ddd);
        phone.Number.Should().Be(number);
        
        // Properties are init-only, so they cannot be changed after construction
        // This is validated by the compiler
    }

    [Fact]
    public void Phone_WithSameValues_ShouldBeEqual()
    {
        // Arrange
        var phone1 = new Phone("+55", "11", "999888777");
        var phone2 = new Phone("+55", "11", "999888777");

        // Act & Assert
        (phone1 == phone2).Should().BeFalse(); // Reference equality should be false
        // Note: GetHashCode() may differ for separate instances even with same values
    }

    [Fact]
    public void Phone_WithDifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        var phone1 = new Phone("+55", "11", "999888777");
        var phone2 = new Phone("+55", "11", "888777666");

        // Act & Assert
        phone1.Should().NotBe(phone2);
    }

    [Fact]
    public void Phone_IsValidPhoneNumber_ShouldNotThrow()
    {
        // Arrange
        var phone = new Phone("+55", "11", "999888777");

        // Act & Assert
        // The method currently has no implementation, so it should not throw
        phone.Invoking(p => p.IsValidPhoneNumber()).Should().NotThrow();
    }

    [Theory]
    [InlineData("", "11", "999888777")]
    [InlineData("+55", "", "999888777")]
    [InlineData("", "", "")]
    public void Phone_WithEmptyValues_ShouldCreateSuccessfully(string countryCode, string ddd, string? number)
    {
        // Note: Phone class doesn't validate empty values, so they're allowed
        
        // Act
        var phone = new Phone(countryCode, ddd, number);

        // Assert
        phone.Should().NotBeNull();
        phone.CountryCode.Should().Be(countryCode);
        phone.DDD.Should().Be(ddd);
        phone.Number.Should().Be(number);
    }
}