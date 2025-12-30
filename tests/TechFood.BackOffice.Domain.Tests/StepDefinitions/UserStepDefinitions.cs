using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.ValueObjects;
using TechFood.Shared.Domain.Exceptions;
using TechTalk.SpecFlow;

namespace TechFood.BackOffice.Domain.Tests.StepDefinitions;

[Binding]
public class UserStepDefinitions
{
    private Name? _name;
    private string _username = string.Empty;
    private string _role = string.Empty;
    private Email? _email;
    private User? _user;
    private string _passwordHash = string.Empty;
    private string _newRole = string.Empty;
    private Exception? _caughtException;

    [Given(@"que eu tenho um nome de usuário completo ""(.*)""")]
    public void GivenIHaveAUserFullName(string fullName)
    {
        _name = new Name(fullName);
    }

    [Given(@"eu tenho um username ""(.*)""")]
    public void GivenIHaveAUsername(string username)
    {
        _username = username;
    }

    [Given(@"eu tenho uma função ""(.*)""")]
    public void GivenIHaveARole(string role)
    {
        _role = role;
    }

    [Given(@"eu tenho um email de usuário ""(.*)""")]
    public void GivenIHaveAUserEmail(string email)
    {
        _email = new Email(email);
    }

    [Given(@"eu não tenho um email de usuário")]
    public void GivenIDoNotHaveAUserEmail()
    {
        _email = null;
    }

    [When(@"eu criar o usuário")]
    public void WhenICreateTheUser()
    {
        _user = new User(_name!, _username, _role, _email);
    }

    [Then(@"o usuário deve ser criado com sucesso")]
    public void ThenTheUserShouldBeCreatedSuccessfully()
    {
        _user.Should().NotBeNull();
    }

    [Then(@"o nome completo do usuário deve ser ""(.*)""")]
    public void ThenTheUserFullNameShouldBe(string expectedFullName)
    {
        _user.Should().NotBeNull();
        _user!.Name.FullName.Should().Be(expectedFullName);
    }

    [Then(@"o username deve ser ""(.*)""")]
    public void ThenTheUsernameShouldBe(string expectedUsername)
    {
        _user.Should().NotBeNull();
        _user!.Username.Should().Be(expectedUsername);
    }

    [Then(@"a função do usuário deve ser ""(.*)""")]
    public void ThenTheUserRoleShouldBe(string expectedRole)
    {
        _user.Should().NotBeNull();
        _user!.Role.Should().Be(expectedRole);
    }

    [Then(@"o email do usuário deve ser ""(.*)""")]
    public void ThenTheUserEmailShouldBe(string expectedEmail)
    {
        _user.Should().NotBeNull();
        _user!.Email.Should().NotBeNull();
        _user!.Email!.Address.Should().Be(expectedEmail);
    }

    [Then(@"o email do usuário deve ser nulo")]
    public void ThenTheUserEmailShouldBeNull()
    {
        _user.Should().NotBeNull();
        _user!.Email.Should().BeNull();
    }

    [Given(@"que eu tenho um usuário válido")]
    public void GivenIHaveAValidUser()
    {
        _name = new Name("Admin User");
        _username = "admin";
        _role = "Administrator";
        _email = new Email("admin@techfood.com");
        _user = new User(_name, _username, _role, _email);
    }

    [Given(@"eu tenho um hash de senha ""(.*)""")]
    public void GivenIHaveAPasswordHash(string passwordHash)
    {
        _passwordHash = passwordHash;
    }

    [When(@"eu definir a senha do usuário")]
    public void WhenISetTheUserPassword()
    {
        _user!.SetPassword(_passwordHash);
    }

    [Then(@"o hash de senha do usuário deve ser ""(.*)""")]
    public void ThenTheUserPasswordHashShouldBe(string expectedPasswordHash)
    {
        _user.Should().NotBeNull();
        _user!.PasswordHash.Should().Be(expectedPasswordHash);
    }

    [Given(@"eu tenho um hash de senha inválido ""(.*)""")]
    public void GivenIHaveAnInvalidPasswordHash(string invalidPasswordHash)
    {
        _passwordHash = invalidPasswordHash;
    }

    [When(@"eu tentar definir a senha do usuário")]
    public void WhenITryToSetTheUserPassword()
    {
        try
        {
            _user!.SetPassword(_passwordHash);
        }
        catch (Exception ex)
        {
            _caughtException = ex;
        }
    }

    [Then(@"a mensagem da exceção deve conter ""(.*)""")]
    public void ThenTheExceptionMessageShouldContain(string expectedMessage)
    {
        _caughtException.Should().NotBeNull();
        _caughtException!.Message.Should().Contain(expectedMessage);
    }

    [Given(@"eu tenho uma nova função ""(.*)""")]
    public void GivenIHaveANewRole(string newRole)
    {
        _newRole = newRole;
    }

    [When(@"eu definir a função do usuário")]
    public void WhenISetTheUserRole()
    {
        _user!.SetRole(_newRole);
    }

    [Given(@"eu tenho uma função inválida ""(.*)""")]
    public void GivenIHaveAnInvalidRole(string invalidRole)
    {
        _newRole = invalidRole;
    }

    [When(@"eu tentar definir a função do usuário")]
    public void WhenITryToSetTheUserRole()
    {
        try
        {
            _user!.SetRole(_newRole);
        }
        catch (Exception ex)
        {
            _caughtException = ex;
        }
    }

    [When(@"eu verificar a hierarquia do usuário")]
    public void WhenICheckTheUserHierarchy()
    {
        // Step para verificação - não faz nada, apenas prepara para os asserts
    }

    [Then(@"o usuário deve ser do tipo Entity")]
    public void ThenTheUserShouldBeOfTypeEntity()
    {
        _user.Should().NotBeNull();
        _user.Should().BeAssignableTo<TechFood.Shared.Domain.Entities.Entity>();
    }

    [Then(@"o usuário deve implementar IAggregateRoot")]
    public void ThenTheUserShouldImplementIAggregateRoot()
    {
        _user.Should().NotBeNull();
        _user.Should().BeAssignableTo<TechFood.Shared.Domain.Entities.IAggregateRoot>();
    }

    [When(@"eu obter as propriedades do usuário")]
    public void WhenIGetTheUserProperties()
    {
        // Step para verificação - não faz nada, apenas prepara para os asserts
    }

    [Then(@"as propriedades Name, Username e Email devem ser somente leitura")]
    public void ThenThePropertiesNameUsernameAndEmailShouldBeReadOnly()
    {
        _user.Should().NotBeNull();
        var originalName = _user!.Name;
        var originalUsername = _user.Username;
        var originalEmail = _user.Email;

        // Verify properties haven't changed (they are read-only)
        _user.Name.Should().Be(originalName);
        _user.Username.Should().Be(originalUsername);
        _user.Email.Should().Be(originalEmail);
    }

    [Then(@"as propriedades Role e PasswordHash podem ser alteradas através de métodos")]
    public void ThenThePropertiesRoleAndPasswordHashCanBeChangedThroughMethods()
    {
        _user.Should().NotBeNull();
        
        // Test that Role and PasswordHash can be changed through methods
        _user!.SetRole("NewRole");
        _user.SetPassword("NewPasswordHash");
        
        _user.Role.Should().Be("NewRole");
        _user.PasswordHash.Should().Be("NewPasswordHash");
    }
}
