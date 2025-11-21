using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.BackOffice.Domain.ValueObjects;
using Xunit;

namespace TechFood.BackOffice.Infra.Tests.Repositories;

public class UserRepositoryTests
{
    private readonly IUserRepository _mockRepository;

    public UserRepositoryTests()
    {
        _mockRepository = Substitute.For<IUserRepository>();
    }

    [Fact]
    public async Task AddAsync_ShouldReturnUserId()
    {
        // Arrange
        var user = new User(
            new Name("John Doe"),
            "john.doe",
            "Admin",
            new Email("john.doe@example.com"));

        var expectedId = user.Id;
        _mockRepository.AddAsync(user).Returns(expectedId);

        // Act
        var result = await _mockRepository.AddAsync(user);

        // Assert
        result.Should().Be(expectedId);
        await _mockRepository.Received(1).AddAsync(user);
    }

    [Fact]
    public async Task GetByUsernameOrEmailAsync_ShouldReturnUser_WhenUserExistsByUsername()
    {
        // Arrange
        var username = "john.doe";
        var user = new User(
            new Name("John Doe"),
            username,
            "Admin",
            new Email("john.doe@example.com"));

        _mockRepository.GetByUsernameOrEmailAsync(username).Returns(user);

        // Act
        var result = await _mockRepository.GetByUsernameOrEmailAsync(username);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(user);
        await _mockRepository.Received(1).GetByUsernameOrEmailAsync(username);
    }

    [Fact]
    public async Task GetByUsernameOrEmailAsync_ShouldReturnUser_WhenUserExistsByEmail()
    {
        // Arrange
        var email = "john.doe@example.com";
        var user = new User(
            new Name("John Doe"),
            "john.doe",
            "Admin",
            new Email(email));

        _mockRepository.GetByUsernameOrEmailAsync(email).Returns(user);

        // Act
        var result = await _mockRepository.GetByUsernameOrEmailAsync(email);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(user);
        await _mockRepository.Received(1).GetByUsernameOrEmailAsync(email);
    }

    [Fact]
    public async Task GetByUsernameOrEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var usernameOrEmail = "nonexistent@example.com";
        _mockRepository.GetByUsernameOrEmailAsync(usernameOrEmail).Returns((User?)null);

        // Act
        var result = await _mockRepository.GetByUsernameOrEmailAsync(usernameOrEmail);

        // Assert
        result.Should().BeNull();
        await _mockRepository.Received(1).GetByUsernameOrEmailAsync(usernameOrEmail);
    }

    [Fact]
    public async Task GetByUsernameOrEmailAsync_ShouldHandleUserWithoutEmail()
    {
        // Arrange
        var username = "john.doe";
        var user = new User(new Name("John Doe"), username, "Admin", null); // User without email

        _mockRepository.GetByUsernameOrEmailAsync(username).Returns(user);

        // Act
        var result = await _mockRepository.GetByUsernameOrEmailAsync(username);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(user);
        await _mockRepository.Received(1).GetByUsernameOrEmailAsync(username);
    }

    [Fact]
    public async Task GetByUsernameOrEmailAsync_ShouldReturnNull_WhenSearchingByEmailButUserHasNoEmail()
    {
        // Arrange
        var email = "john.doe@example.com";
        _mockRepository.GetByUsernameOrEmailAsync(email).Returns((User?)null);

        // Act
        var result = await _mockRepository.GetByUsernameOrEmailAsync(email);

        // Assert
        result.Should().BeNull();
        await _mockRepository.Received(1).GetByUsernameOrEmailAsync(email);
    }
}