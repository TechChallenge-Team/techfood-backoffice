using System;

namespace TechFood.BackOffice.Application.Categories.Dto;

public class CategoryDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;
}
