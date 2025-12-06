using TechFood.BackOffice.Domain.Enums;
using TechFood.Shared.Domain.Exceptions;
using TechFood.Shared.Domain.Validations;
using TechFood.Shared.Domain.ValueObjects;

namespace TechFood.BackOffice.Domain.ValueObjects;

public class Document : ValueObject
{
    public Document(
        DocumentType type,
        string value)
    {
        Type = type;
        Value = value;
        Validate();
    }
    public DocumentType Type { get; set; }

    public string Value { get; set; }

    private void Validate()
    {
        if (Type == DocumentType.CPF && !ValidaDocument.ValidarCPF(Value))
        {
            throw new DomainException(Resources.Exceptions.Customer_ThrowDocumentCPFInvalid);
        }
    }
}
