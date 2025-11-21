using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Enums;
using TechFood.BackOffice.Domain.ValueObjects;

namespace TechFood.BackOffice.Domain.Tests.Fixtures;

public static class CustomerFixture
{
    public static Customer CreateValid(
        string fullName = "João Silva",
        string email = "joao.silva@email.com",
        string documentValue = "11144477735", // Valid CPF
        DocumentType documentType = DocumentType.CPF,
        string? phoneNumber = "999888777")
    {
        var name = new Name(fullName);
        var emailObj = new Email(email);
        var document = new Document(documentType, documentValue);
        var phone = phoneNumber != null ? new Phone("+55", "11", phoneNumber) : null;

        return new Customer(name, emailObj, document, phone);
    }

    public static Customer CreateValidWithoutPhone()
    {
        return CreateValid(
            fullName: "Maria Santos",
            email: "maria.santos@email.com",
            documentValue: "11144477735", // Valid CPF (same as valid for simplicity)
            phoneNumber: null);
    }

    public static Customer CreateWithCustomData(
        Name name,
        Email email,
        Document document,
        Phone? phone = null)
    {
        return new Customer(name, email, document, phone);
    }

    public static Customer CreateCustomerWithDifferentPhone(
        string countryCode = "+1",
        string ddd = "555",
        string phoneNumber = "1234567")
    {
        var name = new Name("John Doe");
        var email = new Email("john.doe@example.com");
        var document = new Document(DocumentType.CPF, "11144477735");
        var phone = new Phone(countryCode, ddd, phoneNumber);

        return new Customer(name, email, document, phone);
    }
}