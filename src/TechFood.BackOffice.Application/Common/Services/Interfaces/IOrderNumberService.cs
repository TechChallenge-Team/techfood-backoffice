using System.Threading.Tasks;

namespace TechFood.BackOffice.Application.Common.Services.Interfaces;

public interface IOrderNumberService
{
    Task<int> GetAsync();
}
