using System;
using System.IO;
using MediatR;
using TechFood.BackOffice.Application.Products.Dto;

namespace TechFood.BackOffice.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    Guid CategoryId,
    Stream ImageFile,
    string ImageContentType,
    decimal Price) : IRequest<ProductDto>;
