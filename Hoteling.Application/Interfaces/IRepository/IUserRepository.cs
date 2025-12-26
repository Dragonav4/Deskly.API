using Hoteling.Domain.Entities;

namespace Hoteling.Application.Interfaces.IRepository;

public interface IUserRepository : ICrudRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
