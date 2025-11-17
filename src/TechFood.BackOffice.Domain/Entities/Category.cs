using TechFood.Shared.Domain.Entities;
using TechFood.Shared.Domain.Validations;

namespace TechFood.BackOffice.Domain.Entities;

public class Category : Entity, IAggregateRoot
{
    public Category() { }

    public Category(string name, string imageFileName, int sortOrder)
    {
        Name = name;
        ImageFileName = imageFileName;
        SortOrder = sortOrder;

        Validate();
    }

    public string Name { get; private set; } = null!;

    public string ImageFileName { get; private set; } = null!;

    public int SortOrder { get; private set; }

    public void UpdateAsync(string name, string imageFileName)
    {
        Name = name;
        ImageFileName = imageFileName;

        Validate();
    }

    private void Validate()
    {
        Validations.ThrowIfEmpty(Name, Resources.Exceptions.Category_ThrowNameIsEmpty);
        Validations.ThrowIfEmpty(ImageFileName, Resources.Exceptions.Category_ThrowFileImageIsEmpty);
        Validations.ThrowIfLessThan(SortOrder, 0, Resources.Exceptions.Category_ThrowIndexIsLessThanZero);
    }
}
