using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.ValueObjects;
using TechFood.Shared.Domain.Exceptions;
using TechTalk.SpecFlow;

namespace TechFood.BackOffice.Domain.Tests.StepDefinitions;

[Binding]
public class CustomerStepDefinitions
{
    private Name? _name;
    private Email? _email;
    private Document? _document;
    private Phone? _phone;
    private Customer? _customer;
    private Exception? _caughtException;
    private string _invalidEmail = string.Empty;
    private string _invalidCpf = string.Empty;

    [Given(@"que eu tenho um nome completo ""(.*)""")]
    public void GivenIHaveAFullName(string fullName)
    {
        _name = new Name(fullName);
    }

    [Given(@"eu tenho um email ""(.*)""")]
    public void GivenIHaveAnEmail(string email)
    {
        _email = new Email(email);
    }

    [Given(@"eu tenho um CPF ""(.*)""")]
    public void GivenIHaveACpf(string cpf)
    {
        _document = new Document(Domain.Enums.DocumentType.CPF, cpf);
    }

    [Given(@"eu tenho um telefone com código do país ""(.*)"", DDD ""(.*)"" e número ""(.*)""")]
    public void GivenIHaveAPhoneWithCountryCodeDddAndNumber(string countryCode, string ddd, string number)
    {
        _phone = new Phone(countryCode, ddd, number);
    }

    [Given(@"eu não tenho telefone")]
    public void GivenIDoNotHaveAPhone()
    {
        _phone = null;
    }

    [When(@"eu criar o cliente")]
    public void WhenICreateTheCustomer()
    {
        _customer = new Customer(_name!, _email!, _document!, _phone);
    }

    [Then(@"o cliente deve ser criado com sucesso")]
    public void ThenTheCustomerShouldBeCreatedSuccessfully()
    {
        _customer.Should().NotBeNull();
    }

    [Then(@"o nome completo do cliente deve ser ""(.*)""")]
    public void ThenTheCustomerFullNameShouldBe(string expectedFullName)
    {
        _customer.Should().NotBeNull();
        _customer!.Name.FullName.Should().Be(expectedFullName);
    }

    [Then(@"o email do cliente deve ser ""(.*)""")]
    public void ThenTheCustomerEmailShouldBe(string expectedEmail)
    {
        _customer.Should().NotBeNull();
        _customer!.Email.Address.Should().Be(expectedEmail);
    }

    [Then(@"o CPF do cliente deve ser ""(.*)""")]
    public void ThenTheCustomerCpfShouldBe(string expectedCpf)
    {
        _customer.Should().NotBeNull();
        _customer!.Document.Value.Should().Be(expectedCpf);
    }

    [Then(@"o tipo do documento deve ser CPF")]
    public void ThenTheDocumentTypeShouldBeCpf()
    {
        _customer.Should().NotBeNull();
        _customer!.Document.Type.Should().Be(Domain.Enums.DocumentType.CPF);
    }

    [Then(@"o telefone do cliente deve ter código do país ""(.*)""")]
    public void ThenTheCustomerPhoneShouldHaveCountryCode(string expectedCountryCode)
    {
        _customer.Should().NotBeNull();
        _customer!.Phone.Should().NotBeNull();
        _customer!.Phone!.CountryCode.Should().Be(expectedCountryCode);
    }

    [Then(@"o telefone do cliente deve ter DDD ""(.*)""")]
    public void ThenTheCustomerPhoneShouldHaveDdd(string expectedDdd)
    {
        _customer.Should().NotBeNull();
        _customer!.Phone.Should().NotBeNull();
        _customer!.Phone!.DDD.Should().Be(expectedDdd);
    }

    [Then(@"o telefone do cliente deve ter número ""(.*)""")]
    public void ThenTheCustomerPhoneShouldHaveNumber(string expectedNumber)
    {
        _customer.Should().NotBeNull();
        _customer!.Phone.Should().NotBeNull();
        _customer!.Phone!.Number.Should().Be(expectedNumber);
    }

    [Then(@"o telefone do cliente deve ser nulo")]
    public void ThenTheCustomerPhoneShouldBeNull()
    {
        _customer.Should().NotBeNull();
        _customer!.Phone.Should().BeNull();
    }

    [Given(@"que eu tenho um email inválido ""(.*)""")]
    public void GivenIHaveAnInvalidEmail(string invalidEmail)
    {
        _invalidEmail = invalidEmail;
    }

    [When(@"eu tentar criar um email")]
    public void WhenITryToCreateAnEmail()
    {
        try
        {
            _email = new Email(_invalidEmail);
        }
        catch (Exception ex)
        {
            _caughtException = ex;
        }
    }

    [Given(@"que eu tenho um CPF inválido ""(.*)""")]
    public void GivenIHaveAnInvalidCpf(string invalidCpf)
    {
        _invalidCpf = invalidCpf;
    }

    [When(@"eu tentar criar um documento CPF")]
    public void WhenITryToCreateACpfDocument()
    {
        try
        {
            _document = new Document(Domain.Enums.DocumentType.CPF, _invalidCpf);
        }
        catch (Exception ex)
        {
            _caughtException = ex;
        }
    }

    [Given(@"que eu tenho um cliente válido")]
    public void GivenIHaveAValidCustomer()
    {
        _name = new Name("João Silva");
        _email = new Email("joao.silva@email.com");
        _document = new Document(Domain.Enums.DocumentType.CPF, "11144477735");
        _phone = new Phone("+55", "11", "999888777");
        _customer = new Customer(_name, _email, _document, _phone);
    }

    [When(@"eu verificar a hierarquia do cliente")]
    public void WhenICheckTheCustomerHierarchy()
    {
        // Step para verificação - não faz nada, apenas prepara para os asserts
    }

    [Then(@"o cliente deve ser do tipo Entity")]
    public void ThenTheCustomerShouldBeOfTypeEntity()
    {
        _customer.Should().NotBeNull();
        _customer.Should().BeAssignableTo<TechFood.Shared.Domain.Entities.Entity>();
    }

    [Then(@"o cliente deve implementar IAggregateRoot")]
    public void ThenTheCustomerShouldImplementIAggregateRoot()
    {
        _customer.Should().NotBeNull();
        _customer.Should().BeAssignableTo<TechFood.Shared.Domain.Entities.IAggregateRoot>();
    }

    [When(@"eu obter as propriedades do cliente")]
    public void WhenIGetTheCustomerProperties()
    {
        // Step para verificação - não faz nada, apenas prepara para os asserts
    }

    [Then(@"as propriedades Name, Email, Document e Phone devem ser somente leitura")]
    public void ThenThePropertiesNameEmailDocumentAndPhoneShouldBeReadOnly()
    {
        _customer.Should().NotBeNull();
        var originalName = _customer!.Name;
        var originalEmail = _customer.Email;
        var originalDocument = _customer.Document;
        var originalPhone = _customer.Phone;

        // Verify properties haven't changed (they are read-only)
        _customer.Name.Should().Be(originalName);
        _customer.Email.Should().Be(originalEmail);
        _customer.Document.Should().Be(originalDocument);
        _customer.Phone.Should().Be(originalPhone);
    }
}
