using System;
using MediatR;

namespace TechFood.BackOffice.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest<Unit>;
