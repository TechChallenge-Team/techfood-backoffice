using System;

namespace TechFood.BackOffice.Application.Menu.Dto;

public class ProductDto
{
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public string ImageUrl { get; set; } = null!;
}
