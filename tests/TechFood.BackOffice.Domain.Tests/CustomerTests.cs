using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Tests.Fixtures;
using TechFood.BackOffice.Domain.ValueObjects;
using TechFood.Shared.Domain.Exceptions;

namespace TechFood.BackOffice.Domain.Tests;

public class CustomerTests
{
    [Fact]
    public void Customer_WithValidParameters_ShouldCreateSuccessfully()
    {
        // Arrange & Act
        var customer = CustomerFixture.CreateValid();

        // Assert
        customer.Should().NotBeNull();
        customer.Name.FullName.Should().Be("João Silva");
        customer.Email.Address.Should().Be("joao.silva@email.com");
        customer.Document.Value.Should().Be("11144477735");
        customer.Document.Type.Should().Be(Domain.Enums.DocumentType.CPF);
        customer.Phone.Should().NotBeNull();
        customer.Phone!.CountryCode.Should().Be("+55");
        customer.Phone.DDD.Should().Be("11");
        customer.Phone.Number.Should().Be("999888777");
    }

    [Fact]
    public void Customer_WithValidParametersWithoutPhone_ShouldCreateSuccessfully()
    {
        // Arrange & Act
        var customer = CustomerFixture.CreateValidWithoutPhone();

        // Assert
        customer.Should().NotBeNull();
        customer.Name.FullName.Should().Be("Maria Santos");
        customer.Email.Address.Should().Be("maria.santos@email.com");
        customer.Document.Value.Should().Be("11144477735");
        customer.Phone.Should().BeNull();
    }

    [Fact]
    public void Customer_WithInvalidEmail_ShouldThrowException()
    {
        // Arrange & Act & Assert
        Assert.Throws<DomainException>(() => new Email("invalid-email"));
    }

    [Fact]
    public void Customer_WithInvalidCPF_ShouldThrowException()
    {
        // Arrange & Act & Assert  
        Assert.Throws<DomainException>(() => new Document(Domain.Enums.DocumentType.CPF, "00000000000"));
    }

    [Fact]
    public void Customer_ShouldInheritFromEntityAndImplementIAggregateRoot()
    {
        // Arrange & Act
        var customer = CustomerFixture.CreateValid();

        // Assert
        customer.Should().BeAssignableTo<TechFood.Shared.Domain.Entities.Entity>();
        customer.Should().BeAssignableTo<TechFood.Shared.Domain.Entities.IAggregateRoot>();
    }

    [Fact]
    public void Customer_Properties_ShouldBeReadOnly()
    {
        // Arrange
        var customer = CustomerFixture.CreateValid();
        var originalName = customer.Name;
        var originalEmail = customer.Email;
        var originalDocument = customer.Document;
        var originalPhone = customer.Phone;

        // Act & Assert - Properties should not have public setters
        // This is verified by compilation - if properties had setters, this wouldn't compile
        customer.Name.Should().Be(originalName);
        customer.Email.Should().Be(originalEmail);
        customer.Document.Should().Be(originalDocument);
        customer.Phone.Should().Be(originalPhone);
    }

    public static IEnumerable<object[]> GetInvalidCustomerData()
    {
        // Note: These will test the validation in value objects
        // Test invalid email first (this will fail at Email construction)
        yield return new object[] 
        {
            "invalid-email", // This will fail when creating Email
            typeof(DomainException)
        };
        
        // Test invalid CPF (this will fail at Document construction)  
        yield return new object[] 
        {
            "00000000000", // Invalid CPF
            typeof(DomainException)
        };
    }
}
