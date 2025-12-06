using System;

namespace TechFood.BackOffice.Application.Products.Dto;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    Guid CategoryId,
    bool OutOfStock,
    string ImageUrl,
    decimal Price);
