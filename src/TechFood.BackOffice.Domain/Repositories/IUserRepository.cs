using System;
using System.Threading.Tasks;
using TechFood.BackOffice.Domain.Entities;

namespace TechFood.BackOffice.Domain.Repositories;

public interface IUserRepository
{
    Task<Guid> AddAsync(User user);

    Task<User?> GetByUsernameOrEmailAsync(string username);
}
