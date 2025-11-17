using Bogus;
using TechFood.Domain.Entities;
using TechFood.Domain.Enums;
using TechFood.Domain.ValueObjects;

namespace TechFood.Doman.Tests.Fixtures;

public class CustomerFixture
{
    private readonly Faker _faker;

    public CustomerFixture()
    {
        _faker = new Faker("pt_BR");
    }
    private readonly string[] _invalidEmails = { "eduardostubbert", "bethania@" };
    public Customer CreateValidCustomer() =>
        new(
            new Name(
                _faker.Name.FullName()),
            new Email(
                _faker.Internet.Email()),
            new Document(DocumentType.CPF, "000.000.001-91"),
            new Phone(
                "+55",
                "11",
                _faker.Phone.PhoneNumber("#####-####"))
            );
    public Customer CreateInvalidCPFCustomer() =>
        new(
            new Name(
                _faker.Name.FullName()),
            new Email(
                _faker.Internet.Email()),
            new Document(DocumentType.CPF, "000.000.004-91"),
            new Phone(
                "+55",
                "11",
                _faker.Phone.PhoneNumber("#####-####"))
            );
    public Customer CreateCustomerEmaiInvalid() =>
        new(
            new Name(
                _faker.Name.FullName()),
            new Email(
                _faker.PickRandom(_invalidEmails)),
            new Document(DocumentType.CPF, "000.000.001-91"),
            new Phone(
                "+55",
                "11",
                _faker.Phone.PhoneNumber("#####-####"))
            );
}
