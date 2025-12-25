using Hoteling.Domain.Entities;
using Hoteling.Infastructure.Data;

namespace Hoteling.Infastructure.Repositories.Users;

public class UserRepository(AppDbContext dbContext) : CrudRepository<User>(dbContext), IUserRepository
{
    
}