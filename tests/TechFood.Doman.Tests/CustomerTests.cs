using TechFood.Domain.Entities;
using TechFood.Shared.Domain.Exceptions;
using TechFood.Doman.Tests.Fixtures;

namespace TechFood.Doman.Tests;

public class CustomerTests : IClassFixture<CustomerFixture>
{
    private readonly CustomerFixture _customerFixture;
    public CustomerTests(CustomerFixture customerFixture)
    { _customerFixture = customerFixture; }

    [Fact(DisplayName = "Create Customer Valid")]
    [Trait("Customer", "Create Customer Valid")]
    public void Create_ValidCustomer()
    {
        // Assert
        var exception = Record.Exception(_customerFixture.CreateValidCustomer);
        Assert.Null(exception);
    }

    [Fact(DisplayName = "Cannot add Customer with invalid document")]
    [Trait("Customer", "Customer Add")]
    public void ShoudThrowException_WhenAddCustomerDocumentCPFInvalid()
    {
        // Act
        var result = Assert.Throws<DomainException>(_customerFixture.CreateInvalidCPFCustomer);
        // Assert
        Assert.Equal(Domain.Resources.Exceptions.Customer_ThrowDocumentCPFInvalid, result.Message);
    }
    [Fact(DisplayName = "Cannot add Customer with invalid email")]
    [Trait("Customer", "Customer Email Invalid")]
    public void ShoudThrowException_WhenAddCustomerEmailInvalid()
    {
        // Act
        var result = Assert.Throws<DomainException>(_customerFixture.CreateCustomerEmaiInvalid);
        // Assert
        Assert.Equal(Domain.Resources.Exceptions.Customer_ThrowEmailInvalid, result.Message);
    }
}
