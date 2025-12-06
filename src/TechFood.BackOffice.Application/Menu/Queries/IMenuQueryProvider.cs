using System.Threading.Tasks;
using TechFood.BackOffice.Application.Menu.Dto;

namespace TechFood.BackOffice.Application.Menu.Queries;

public interface IMenuQueryProvider
{
    Task<MenuDto> GetAsync();
}
