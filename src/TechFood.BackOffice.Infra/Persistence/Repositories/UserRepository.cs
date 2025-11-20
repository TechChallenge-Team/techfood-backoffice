using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using TechFood.BackOffice.Domain.Entities;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.Infra.Persistence.Contexts;

namespace TechFood.Infra.Persistence.Repositories
{
    internal class UserRepository(BackOfficeContext dbContext) : IUserRepository
    {
        private readonly BackOfficeContext _dbContext = dbContext;

        public async Task<Guid> AddAsync(User user)
        {
            var entry = await _dbContext.AddAsync(user);

            return entry.Entity.Id;
        }

        public async Task<User?> GetByUsernameOrEmailAsync(string username)
        {
            return await _dbContext
                .Users
                .FirstOrDefaultAsync(
                    u => u.Username == username || (u.Email != null && u.Email.Address! == username));
        }
    }
}
