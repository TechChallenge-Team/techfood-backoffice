using MediatR;
using Microsoft.AspNetCore.Http;
using TechFood.Lambda.Customers.Controllers;
using TechFood.BackOffice.Application.Customers.Commands.CreateCustomer;
using TechFood.BackOffice.Application.Customers.Dto;
using TechFood.BackOffice.Application.Customers.Queries.GetCustomerByDocument;
using TechFood.BackOffice.Contracts.Customers;
using TechFood.BackOffice.Domain.Enums;

namespace TechFood.BackOffice.Api.Tests.Controllers;

public class CustomersControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CustomersController _controller;

    public CustomersControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new CustomersController(_mediatorMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ShouldReturnOkResult()
    {
        // Arrange
        var request = new CreateCustomerRequest(
            CPF: "12345678901",
            Name: "João Silva",
            Email: "joao.silva@email.com"
        );

        var customerDto = new CustomerDto
        {
            Id = Guid.NewGuid(),
            DocumentType = DocumentType.CPF,
            DocumentValue = request.CPF,
            Name = request.Name,
            Email = request.Email
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), default))
                     .ReturnsAsync(customerDto);

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(customerDto);
        
        _mediatorMock.Verify(m => m.Send(
            It.Is<CreateCustomerCommand>(c => 
                c.CPF == request.CPF && 
                c.Name == request.Name && 
                c.Email == request.Email), 
            default), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateCommandWithCorrectParameters()
    {
        // Arrange
        var request = new CreateCustomerRequest(
            CPF: "98765432100",
            Name: "Maria Santos",
            Email: "maria.santos@email.com"
        );

        var customerDto = new CustomerDto
        {
            Id = Guid.NewGuid(),
            DocumentType = DocumentType.CPF,
            DocumentValue = request.CPF,
            Name = request.Name,
            Email = request.Email
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), default))
                     .ReturnsAsync(customerDto);

        // Act
        await _controller.CreateAsync(request);

        // Assert
        _mediatorMock.Verify(m => m.Send(
            It.Is<CreateCustomerCommand>(command =>
                command.CPF == request.CPF &&
                command.Name == request.Name &&
                command.Email == request.Email),
            default), Times.Once);
    }

    [Fact]
    public async Task GetByDocumentAsync_WithValidDocument_ShouldReturnOkResult()
    {
        // Arrange
        var document = "12345678901";
        var customerDto = new CustomerDto
        {
            Id = Guid.NewGuid(),
            DocumentType = DocumentType.CPF,
            DocumentValue = document,
            Name = "Cliente Teste",
            Email = "cliente@teste.com"
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerByDocumentQuery>(), default))
                     .ReturnsAsync(customerDto);

        // Act
        var result = await _controller.GetByDocumentAsync(document);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(customerDto);
        
        _mediatorMock.Verify(m => m.Send(
            It.Is<GetCustomerByDocumentQuery>(q => 
                q.DocumentType == DocumentType.CPF && 
                q.DocumentValue == document), 
            default), Times.Once);
    }

    [Fact]
    public async Task GetByDocumentAsync_WithNonExistentDocument_ShouldReturnNotFound()
    {
        // Arrange
        var document = "00000000000";

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerByDocumentQuery>(), default))
                     .ReturnsAsync((CustomerDto?)null);

        // Act
        var result = await _controller.GetByDocumentAsync(document);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        
        _mediatorMock.Verify(m => m.Send(
            It.Is<GetCustomerByDocumentQuery>(q => 
                q.DocumentType == DocumentType.CPF && 
                q.DocumentValue == document), 
            default), Times.Once);
    }

    [Fact]
    public async Task GetByDocumentAsync_ShouldCreateQueryWithCorrectParameters()
    {
        // Arrange
        var document = "11122233344";

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerByDocumentQuery>(), default))
                     .ReturnsAsync((CustomerDto?)null);

        // Act
        await _controller.GetByDocumentAsync(document);

        // Assert
        _mediatorMock.Verify(m => m.Send(
            It.Is<GetCustomerByDocumentQuery>(query =>
                query.DocumentType == DocumentType.CPF &&
                query.DocumentValue == document),
            default), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenMediatorReturnsResult_ShouldReturnOkWithResult()
    {
        // Arrange
        var request = new CreateCustomerRequest(
            CPF: "55566677788",
            Name: "Ana Costa",
            Email: "ana.costa@email.com"
        );

        var expectedResult = new CustomerDto
        {
            Id = Guid.NewGuid(),
            DocumentType = DocumentType.CPF,
            DocumentValue = request.CPF,
            Name = request.Name,
            Email = request.Email,
            Phone = null
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), default))
                     .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedResult);
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task GetByDocumentAsync_WhenCustomerExists_ShouldReturnOkWithCustomer()
    {
        // Arrange
        var document = "99988877766";
        var expectedCustomer = new CustomerDto
        {
            Id = Guid.NewGuid(),
            DocumentType = DocumentType.CPF,
            DocumentValue = document,
            Name = "Pedro Oliveira",
            Email = "pedro.oliveira@email.com",
            Phone = "11999888777"
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerByDocumentQuery>(), default))
                     .ReturnsAsync(expectedCustomer);

        // Act
        var result = await _controller.GetByDocumentAsync(document);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedCustomer);
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
}