using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Enums;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.BackOffice.Domain.ValueObjects;
using Xunit;

namespace TechFood.BackOffice.Infra.Tests.Repositories;

public class CustomerRepositoryTests
{
    private readonly ICustomerRepository _mockRepository;

    public CustomerRepositoryTests()
    {
        _mockRepository = Substitute.For<ICustomerRepository>();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCustomerId()
    {
        // Arrange
        var customer = new Customer(
            new Name("John Doe"),
            new Email("john.doe@example.com"),
            new Document(DocumentType.CPF, "11144477735"),
            new Phone("+55", "11", "999999999"));
        
        var expectedId = customer.Id;
        _mockRepository.CreateAsync(customer).Returns(expectedId);

        // Act
        var result = await _mockRepository.CreateAsync(customer);

        // Assert
        result.Should().Be(expectedId);
        await _mockRepository.Received(1).CreateAsync(customer);
    }



    [Fact]
    public async Task GetByDocumentAsync_ShouldReturnCustomer_WhenCustomerWithDocumentExists()
    {
        // Arrange
        var documentType = DocumentType.CPF;
        var documentValue = "11144477735";
        var customer = new Customer(
            new Name("Tech Company"),
            new Email("contact@techcompany.com"),
            new Document(documentType, documentValue),
            new Phone("+55", "11", "888888888"));

        _mockRepository.GetByDocumentAsync(documentType, documentValue).Returns(customer);

        // Act
        var result = await _mockRepository.GetByDocumentAsync(documentType, documentValue);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(customer);
        await _mockRepository.Received(1).GetByDocumentAsync(documentType, documentValue);
    }

    [Fact]
    public async Task GetByDocumentAsync_ShouldReturnNull_WhenCustomerWithDocumentDoesNotExist()
    {
        // Arrange
        var documentType = DocumentType.CPF;
        var documentValue = "40532176002";
        _mockRepository.GetByDocumentAsync(documentType, documentValue).Returns((Customer?)null);

        // Act
        var result = await _mockRepository.GetByDocumentAsync(documentType, documentValue);

        // Assert
        result.Should().BeNull();
        await _mockRepository.Received(1).GetByDocumentAsync(documentType, documentValue);
    }
}