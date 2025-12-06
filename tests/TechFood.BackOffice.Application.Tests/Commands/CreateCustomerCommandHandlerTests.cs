using TechFood.BackOffice.Application.Customers.Commands.CreateCustomer;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Enums;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.BackOffice.Domain.ValueObjects;

namespace TechFood.BackOffice.Application.Tests.Commands;

public class CreateCustomerCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerCommandHandlerTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateCustomerAndReturnDto()
    {
        // Arrange
        var command = new CreateCustomerCommand("11144477735", "João Silva", "joao@example.com");
        var customerId = Guid.NewGuid();

        _customerRepositoryMock
            .Setup(r => r.GetByDocumentAsync(DocumentType.CPF, "11144477735"))
            .ReturnsAsync((Customer?)null);

        _customerRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Customer>()))
            .ReturnsAsync(customerId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(customerId);
        result.DocumentType.Should().Be(DocumentType.CPF);
        result.DocumentValue.Should().Be("11144477735");
        result.Name.Should().Be("João Silva");
        result.Email.Should().Be("joao@example.com");
        result.Phone.Should().BeNull();

        _customerRepositoryMock.Verify(
            r => r.GetByDocumentAsync(DocumentType.CPF, "11144477735"),
            Times.Once);
        _customerRepositoryMock.Verify(
            r => r.CreateAsync(It.IsAny<Customer>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCpfAlreadyExists_ShouldThrowApplicationException()
    {
        // Arrange
        var command = new CreateCustomerCommand("11144477735", "Maria Santos", "maria@example.com");
        var existingCustomer = new Customer(
            new Name("Cliente Existente"),
            new Email("existente@example.com"),
            new Document(DocumentType.CPF, "11144477735"),
            null
        );

        _customerRepositoryMock
            .Setup(r => r.GetByDocumentAsync(DocumentType.CPF, "11144477735"))
            .ReturnsAsync(existingCustomer);

        // Act & Assert
        await Assert.ThrowsAsync<TechFood.Shared.Application.Exceptions.ApplicationException>(
            async () => await _handler.Handle(command, CancellationToken.None));

        _customerRepositoryMock.Verify(
            r => r.GetByDocumentAsync(DocumentType.CPF, "11144477735"),
            Times.Once);
        _customerRepositoryMock.Verify(
            r => r.CreateAsync(It.IsAny<Customer>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCreateCustomerWithCorrectProperties()
    {
        // Arrange
        var command = new CreateCustomerCommand("52998224725", "Pedro Oliveira", "pedro@example.com");
        var customerId = Guid.NewGuid();
        Customer? capturedCustomer = null;

        _customerRepositoryMock
            .Setup(r => r.GetByDocumentAsync(DocumentType.CPF, "11144477735"))
            .ReturnsAsync((Customer?)null);

        _customerRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Customer>()))
            .Callback<Customer>(c => capturedCustomer = c)
            .ReturnsAsync(customerId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedCustomer.Should().NotBeNull();
        capturedCustomer!.Name.FullName.Should().Be("Pedro Oliveira");
        capturedCustomer.Email.Address.Should().Be("pedro@example.com");
        capturedCustomer.Document.Type.Should().Be(DocumentType.CPF);
        capturedCustomer.Document.Value.Should().Be("52998224725");
        capturedCustomer.Phone.Should().BeNull();
    }

    [Theory]
    [InlineData("52998224725", "Ana Costa", "ana@example.com")]
    [InlineData("00000000191", "Carlos Lima", "carlos@example.com")]
    [InlineData("52998224725", "Julia Santos", "julia@example.com")]
    public async Task Handle_WithDifferentValidInputs_ShouldCreateCustomer(string cpf, string name, string email)
    {
        // Arrange
        var command = new CreateCustomerCommand(cpf, name, email);
        var customerId = Guid.NewGuid();

        _customerRepositoryMock
            .Setup(r => r.GetByDocumentAsync(DocumentType.CPF, cpf))
            .ReturnsAsync((Customer?)null);

        _customerRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Customer>()))
            .ReturnsAsync(customerId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.DocumentValue.Should().Be(cpf);
        result.Name.Should().Be(name);
        result.Email.Should().Be(email);
    }

    [Fact]
    public async Task Handle_ShouldCheckIfCpfExistsBeforeCreating()
    {
        // Arrange
        var command = new CreateCustomerCommand("52998224725", "Roberto Silva", "roberto@example.com");
        var customerId = Guid.NewGuid();

        _customerRepositoryMock
            .Setup(r => r.GetByDocumentAsync(DocumentType.CPF, "52998224725"))
            .ReturnsAsync((Customer?)null);

        _customerRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Customer>()))
            .ReturnsAsync(customerId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _customerRepositoryMock.Verify(
            r => r.GetByDocumentAsync(DocumentType.CPF, "52998224725"),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCustomerIsCreated_ShouldReturnDtoWithCorrectId()
    {
        // Arrange
        var command = new CreateCustomerCommand("00000000191", "Fernanda Lima", "fernanda@example.com");
        var expectedId = Guid.NewGuid();

        _customerRepositoryMock
            .Setup(r => r.GetByDocumentAsync(DocumentType.CPF, "00000000191"))
            .ReturnsAsync((Customer?)null);

        _customerRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Customer>()))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Id.Should().Be(expectedId);
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldComplete()
    {
        // Arrange
        var command = new CreateCustomerCommand("00000000191", "Marcos Souza", "marcos@example.com");
        var customerId = Guid.NewGuid();
        var cancellationToken = new CancellationToken();

        _customerRepositoryMock
            .Setup(r => r.GetByDocumentAsync(DocumentType.CPF, "00000000191"))
            .ReturnsAsync((Customer?)null);

        _customerRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Customer>()))
            .ReturnsAsync(customerId);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(customerId);
    }

    [Fact]
    public async Task Handle_ShouldReturnDtoWithNullPhone()
    {
        // Arrange
        var command = new CreateCustomerCommand("00000000191", "Patricia Rocha", "patricia@example.com");
        var customerId = Guid.NewGuid();

        _customerRepositoryMock
            .Setup(r => r.GetByDocumentAsync(DocumentType.CPF, "55555555565"))
            .ReturnsAsync((Customer?)null);

        _customerRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Customer>()))
            .ReturnsAsync(customerId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Phone.Should().BeNull();
    }
}
