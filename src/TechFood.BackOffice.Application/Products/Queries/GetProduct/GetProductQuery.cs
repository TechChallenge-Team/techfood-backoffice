using System;
using MediatR;
using TechFood.BackOffice.Application.Products.Dto;

namespace TechFood.BackOffice.Application.Products.Queries.GetProduct;

public record GetProductQuery(Guid Id) : IRequest<ProductDto?>;
