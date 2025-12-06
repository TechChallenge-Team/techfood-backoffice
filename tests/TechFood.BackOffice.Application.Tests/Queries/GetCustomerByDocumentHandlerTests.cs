using TechFood.BackOffice.Application.Customers.Queries.GetCustomerByDocument;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Enums;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.BackOffice.Domain.ValueObjects;

namespace TechFood.BackOffice.Application.Tests.Queries;

public class GetCustomerByDocumentHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly GetCustomerByDocumentHandler _handler;

    public GetCustomerByDocumentHandlerTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _handler = new GetCustomerByDocumentHandler(_customerRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCustomerExists_ShouldReturnCustomerDto()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var document = new Document(DocumentType.CPF, "11144477735");
        var customer = new Customer(
            new Name("João Silva"),
            new Email("joao@example.com"),
            document,
            new Phone("55", "11", "987654321")
        );
        
        // Set the Id using reflection since it's likely protected
        typeof(Customer).GetProperty("Id")!.SetValue(customer, customerId);

        var query = new GetCustomerByDocumentQuery(DocumentType.CPF, "11144477735");

        _customerRepositoryMock
            .Setup(r => r.GetByDocumentAsync(DocumentType.CPF, "11144477735"))
            .ReturnsAsync(customer);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(customerId);
        result.DocumentType.Should().Be(DocumentType.CPF);
        result.DocumentValue.Should().Be("11144477735");
        result.Name.Should().Be("João Silva");
        result.Email.Should().Be("joao@example.com");
        result.Phone.Should().Be("987654321");

        _customerRepositoryMock.Verify(
            r => r.GetByDocumentAsync(DocumentType.CPF, "11144477735"),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCustomerDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var query = new GetCustomerByDocumentQuery(DocumentType.CPF, "00000000000");

        _customerRepositoryMock
            .Setup(r => r.GetByDocumentAsync(DocumentType.CPF, "00000000000"))
            .ReturnsAsync((Customer?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();

        _customerRepositoryMock.Verify(
            r => r.GetByDocumentAsync(DocumentType.CPF, "00000000000"),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCustomerHasNoPhone_ShouldReturnDtoWithNullPhone()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var document = new Document(DocumentType.CPF, "98765432100");
        var customer = new Customer(
            new Name("Maria Santos"),
            new Email("maria@example.com"),
            document,
            null
        );
        
        typeof(Customer).GetProperty("Id")!.SetValue(customer, customerId);

        var query = new GetCustomerByDocumentQuery(DocumentType.CPF, "98765432100");

        _customerRepositoryMock
            .Setup(r => r.GetByDocumentAsync(DocumentType.CPF, "98765432100"))
            .ReturnsAsync(customer);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(customerId);
        result.Phone.Should().BeNull();
        result.Name.Should().Be("Maria Santos");
        result.Email.Should().Be("maria@example.com");

        _customerRepositoryMock.Verify(
            r => r.GetByDocumentAsync(DocumentType.CPF, "98765432100"),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange
        var documentType = DocumentType.CPF;
        var documentValue = "11122233344";
        var query = new GetCustomerByDocumentQuery(documentType, documentValue);

        _customerRepositoryMock
            .Setup(r => r.GetByDocumentAsync(documentType, documentValue))
            .ReturnsAsync((Customer?)null);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _customerRepositoryMock.Verify(
            r => r.GetByDocumentAsync(
                It.Is<DocumentType>(dt => dt == documentType),
                It.Is<string>(dv => dv == documentValue)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldComplete()
    {
        // Arrange
        var query = new GetCustomerByDocumentQuery(DocumentType.CPF, "55566677788");
        var cancellationToken = new CancellationToken();

        _customerRepositoryMock
            .Setup(r => r.GetByDocumentAsync(DocumentType.CPF, "55566677788"))
            .ReturnsAsync((Customer?)null);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().BeNull();
        _customerRepositoryMock.Verify(
            r => r.GetByDocumentAsync(DocumentType.CPF, "55566677788"),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var query = new GetCustomerByDocumentQuery(DocumentType.CPF, "99988877766");

        _customerRepositoryMock
            .Setup(r => r.GetByDocumentAsync(DocumentType.CPF, "99988877766"))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            async () => await _handler.Handle(query, CancellationToken.None));

        _customerRepositoryMock.Verify(
            r => r.GetByDocumentAsync(DocumentType.CPF, "99988877766"),
            Times.Once);
    }
}
