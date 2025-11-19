using System;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Enums;

namespace TechFood.BackOffice.Application.Customers.Dto;

public class CustomerDto
{
    public Guid Id { get; set; }

    public DocumentType DocumentType { get; set; }

    public string DocumentValue { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; } = null!;

    public CustomerDto ConvertToDto(Customer customer)
    {
        Id = customer.Id;
        DocumentType = customer.Document.Type;
        DocumentValue = customer.Document.Value;
        Name = customer.Name.FullName;
        Email = customer.Email.Address;
        Phone = customer.Phone?.Number;

        return this;
    }
}
