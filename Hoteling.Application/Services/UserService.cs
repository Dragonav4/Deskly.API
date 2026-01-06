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
        var existingUserWithSameUsername = await repository.GetByUsernameAsync(updateDto.UserName);
        if (existingUserWithSameUsername != null && existingUserWithSameUsername.Id != updateDto.Id)
        {
            throw new ConflictException($"Username '{updateDto.UserName}' is already taken by another user.");
        }

        return await base.UpdateAsync(updateDto, cancellationToken);
    }

    public async override Task<User> CreateAsync(User createModelDto, CancellationToken cancellationToken = default)
    {
        await userValidateAsync(createModelDto);
        return await base.CreateAsync(createModelDto, cancellationToken);
    }

    private async Task userValidateAsync(User createModelDto)
    {
        var existingUserWithSameUsername = await repository.GetByUsernameAsync(createModelDto.UserName);
        var existingUserWithSameEmail = await repository.GetByEmailAsync(createModelDto.Email);
        if(existingUserWithSameUsername != null) throw new ConflictException($"Username '{createModelDto.UserName}' is already taken by another user.");
        if(existingUserWithSameEmail != null) throw new ConflictException($"Email '{createModelDto.Email}' is already taken by another user.");
    }
}

