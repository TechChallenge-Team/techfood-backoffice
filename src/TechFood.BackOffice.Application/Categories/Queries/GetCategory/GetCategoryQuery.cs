using System;
using MediatR;
using TechFood.BackOffice.Application.Categories.Dto;

namespace TechFood.BackOffice.Application.Categories.Queries.GetCategory;

public record GetCategoryQuery(Guid Id) : IRequest<CategoryDto?>;
