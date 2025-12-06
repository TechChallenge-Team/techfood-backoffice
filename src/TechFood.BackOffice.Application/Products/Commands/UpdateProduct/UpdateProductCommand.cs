using System;
using System.IO;
using MediatR;
using TechFood.BackOffice.Application.Products.Dto;

namespace TechFood.BackOffice.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    Guid CategoryId,
    decimal Price,
    Stream? ImageFile,
    string? ImageContentType
    ) : IRequest<ProductDto>;
