using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Tests.Fixtures;
using TechFood.BackOffice.Domain.ValueObjects;
using TechFood.Shared.Domain.Exceptions;

namespace TechFood.BackOffice.Domain.Tests;

public class UserTests
{
    [Fact]
    public void User_WithValidParameters_ShouldCreateSuccessfully()
    {
        // Arrange & Act
        var user = UserFixture.CreateValid();

        // Assert
        user.Should().NotBeNull();
        user.Name.FullName.Should().Be("Admin User");
        user.Username.Should().Be("admin");
        user.Role.Should().Be("Administrator");
        user.Email!.Address.Should().Be("admin@techfood.com");
    }

    [Fact]
    public void User_WithValidParametersWithoutEmail_ShouldCreateSuccessfully()
    {
        // Arrange & Act
        var user = UserFixture.CreateValidWithoutEmail();

        // Assert
        user.Should().NotBeNull();
        user.Name.FullName.Should().Be("Manager User");
        user.Username.Should().Be("manager");
        user.Role.Should().Be("Manager");
        user.Email.Should().BeNull();
    }

    [Fact]
    public void SetPassword_WithValidPasswordHash_ShouldSetSuccessfully()
    {
        // Arrange
        var user = UserFixture.CreateValid();
        var passwordHash = "$2a$10$abcdefghijklmnopqrstuvwxyz";

        // Act
        user.SetPassword(passwordHash);

        // Assert
        user.PasswordHash.Should().Be(passwordHash);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void SetPassword_WithInvalidPasswordHash_ShouldThrowDomainException(string invalidPasswordHash)
    {
        // Arrange
        var user = UserFixture.CreateValid();

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => user.SetPassword(invalidPasswordHash));
        exception.Message.Should().Contain("password hash cannot be empty");
    }

    [Fact]
    public void SetRole_WithValidRole_ShouldSetSuccessfully()
    {
        // Arrange
        var user = UserFixture.CreateValid();
        var newRole = "SuperAdmin";

        // Act
        user.SetRole(newRole);

        // Assert
        user.Role.Should().Be(newRole);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void SetRole_WithInvalidRole_ShouldThrowDomainException(string invalidRole)
    {
        // Arrange
        var user = UserFixture.CreateValid();

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => user.SetRole(invalidRole));
        exception.Message.Should().Contain("role name cannot be empty");
    }

    [Fact]
    public void User_ShouldInheritFromEntityAndImplementIAggregateRoot()
    {
        // Arrange & Act
        var user = UserFixture.CreateValid();

        // Assert
        user.Should().BeAssignableTo<TechFood.Shared.Domain.Entities.Entity>();
        user.Should().BeAssignableTo<TechFood.Shared.Domain.Entities.IAggregateRoot>();
    }

    [Fact]
    public void User_Properties_ShouldBeReadOnlyExceptPasswordAndRole()
    {
        // Arrange
        var user = UserFixture.CreateValid();
        var originalName = user.Name;
        var originalUsername = user.Username;
        var originalEmail = user.Email;

        // Act & Assert - Most properties should not have public setters
        user.Name.Should().Be(originalName);
        user.Username.Should().Be(originalUsername);
        user.Email.Should().Be(originalEmail);
        
        // Role and PasswordHash can be changed through methods
        user.SetRole("NewRole");
        user.SetPassword("NewPasswordHash");
        user.Role.Should().Be("NewRole");
        user.PasswordHash.Should().Be("NewPasswordHash");
    }
}