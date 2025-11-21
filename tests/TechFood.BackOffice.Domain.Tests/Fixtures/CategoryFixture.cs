using TechFood.BackOffice.Domain.Entities;

namespace TechFood.BackOffice.Domain.Tests.Fixtures;

public static class CategoryFixture
{
    public static Category CreateValid(string name = "Lanche", string imageFileName = "lanche.png", int sortOrder = 0)
    {
        return new Category(name, imageFileName, sortOrder);
    }

    public static Category CreateLanche()
    {
        return new Category("Lanche", "lanche.png", 0);
    }

    public static Category CreateBebida()
    {
        return new Category("Bebida", "bebida.png", 1);
    }

    public static Category CreateAcompanhamento()
    {
        return new Category("Acompanhamento", "acompanhamento.png", 2);
    }

    public static Category CreateSobremesa()
    {
        return new Category("Sobremesa", "sobremesa.png", 3);
    }

    public static List<Category> CreateMultiple()
    {
        return new List<Category>
        {
            CreateLanche(),
            CreateBebida(),
            CreateAcompanhamento(),
            CreateSobremesa()
        };
    }

    public static Category CreateWithCustomSortOrder(int sortOrder)
    {
        return new Category($"Category {sortOrder}", $"category_{sortOrder}.png", sortOrder);
    }
}