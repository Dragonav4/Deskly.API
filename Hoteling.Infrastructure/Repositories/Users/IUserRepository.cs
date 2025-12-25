using Hoteling.Application.Interfaces.IRepository;
using Hoteling.Domain.Entities;

namespace Hoteling.Infastructure.Repositories.Users;

public interface IUserRepository : ICrudRepository<User>
{
    
}