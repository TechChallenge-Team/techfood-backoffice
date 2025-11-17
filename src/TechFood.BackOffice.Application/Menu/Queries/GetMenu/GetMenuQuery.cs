using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.BackOffice.Application.Menu.Dto;

namespace TechFood.BackOffice.Application.Menu.Queries.GetMenu
{
    public class GetMenuQuery : IRequest<MenuDto>
    {
        public class Handler(IMenuQueryProvider queries) : IRequestHandler<GetMenuQuery, MenuDto>
        {
            public Task<MenuDto> Handle(GetMenuQuery request, CancellationToken cancellationToken)
                => queries.GetAsync();
        }
    }
}
