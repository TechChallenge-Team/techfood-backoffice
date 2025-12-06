using System;
using System.Collections.Generic;

namespace TechFood.BackOffice.Application.Menu.Dto;

public class CategoryDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public int SortOrder { get; set; }

    public List<ProductDto> Products { get; set; } = [];
}
