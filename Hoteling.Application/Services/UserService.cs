using Hoteling.Application.Interfaces.IRepository;
using Hoteling.Application.Interfaces.IService;
using Hoteling.Domain.Entities;

namespace Hoteling.Application.Services;

public class UserService(IUserRepository repository) : CrudService<User>(repository), IUserService
{
    public async Task<User?> GetUserByEmail(string email)
    {
        return await repository.GetByEmailAsync(email);
    }


}

public interface IUserService : IService<User>
{
    Task<User?> GetUserByEmail(string email);
}
