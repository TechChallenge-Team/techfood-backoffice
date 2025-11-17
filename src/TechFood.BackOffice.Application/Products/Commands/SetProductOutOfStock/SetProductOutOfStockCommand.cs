using System;
using MediatR;
using TechFood.BackOffice.Application.Products.Dto;

namespace TechFood.BackOffice.Application.Products.Commands.SetProductOutOfStock;

public record SetProductOutOfStockCommand(Guid Id, bool OutOfStock) : IRequest<ProductDto>;
