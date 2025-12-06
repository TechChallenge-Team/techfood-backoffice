using System.Collections.Generic;
using MediatR;
using TechFood.BackOffice.Application.Categories.Dto;

namespace TechFood.BackOffice.Application.Categories.Queries.ListCategories;

public record ListCategoriesQuery : IRequest<List<CategoryDto>>;
