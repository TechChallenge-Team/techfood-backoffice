using System;
using MediatR;

namespace TechFood.BackOffice.Application.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : IRequest<Unit>;
