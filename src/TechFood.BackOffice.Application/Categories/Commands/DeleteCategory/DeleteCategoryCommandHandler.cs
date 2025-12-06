using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.BackOffice.Application.Common.Resources;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.Shared.Application.Exceptions;

namespace TechFood.BackOffice.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(ICategoryRepository repo) : IRequestHandler<DeleteCategoryCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await repo.GetByIdAsync(request.Id);
        if (category == null)
        {
            throw new ApplicationException(Exceptions.Category_CategoryNotFound);
        }

        await repo.DeleteAsync(category);

        return Unit.Value;
    }
}

