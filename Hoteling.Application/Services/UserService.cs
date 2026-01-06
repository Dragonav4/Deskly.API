using Hoteling.Application.Exceptions;
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

    public async override Task<User?> UpdateAsync(User updateDto, CancellationToken cancellationToken = default)
    {
        var user = await repository.GetByIdAsync(updateDto.Id, cancellationToken);
        if (user!.Role == updateDto.Role) throw new UserRoleException("User already has this role.");
        if(user.UserName == updateDto.UserName) throw new UserNameException("User name already exists.");
        return await base.UpdateAsync(updateDto, cancellationToken);
    }
}

