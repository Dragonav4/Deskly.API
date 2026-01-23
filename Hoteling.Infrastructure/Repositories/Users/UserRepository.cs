using Hoteling.Application.Interfaces.IRepository;
using Hoteling.Domain.Entities;
using Hoteling.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hoteling.Infrastructure.Repositories.Users;


public class UserRepository(AppDbContext dbContext) : CrudRepository<User>(dbContext), IUserRepository
{

    public async Task<User?> GetByEmailAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
            throw new ArgumentNullException(nameof(email), "Email is required.");

        return await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}
