using TechFood.BackOffice.Domain.ValueObjects;
using TechFood.Shared.Domain.Entities;

namespace TechFood.BackOffice.Domain.Entities;

public class Customer : Entity, IAggregateRoot
{
    private Customer() { }

    public Customer(Name name, Email email, Document document, Phone? phone)
    {
        Name = name;
        Email = email;
        Document = document;
        Phone = phone;
    }

    public Name Name { get; private set; } = null!;

    public Email Email { get; private set; } = null!;

    public Document Document { get; private set; } = null!;

    public Phone? Phone { get; private set; } = null!;
}
