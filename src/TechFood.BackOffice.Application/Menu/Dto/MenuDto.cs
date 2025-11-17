using System.Collections.Generic;

namespace TechFood.BackOffice.Application.Menu.Dto;

public class MenuDto
{
    public IEnumerable<CategoryDto> Categories { get; set; } = [];
}
