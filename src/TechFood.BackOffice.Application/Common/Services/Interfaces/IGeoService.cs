using System.Threading.Tasks;
using TechFood.BackOffice.Application.Common.Data;

namespace TechFood.BackOffice.Application.Common.Services.Interfaces;

public interface IGeoService
{
    Task<Location> GetLocationAsync(Coordinates coordinates);
}
