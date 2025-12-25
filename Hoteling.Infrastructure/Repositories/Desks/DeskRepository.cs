using Hoteling.Domain.Entities;
using Hoteling.Infastructure.Data;

namespace Hoteling.Infastructure.Repositories.Desks;

public class DeskRepository(AppDbContext dbContext) : CrudRepository<Desk>(dbContext), IDeskRepository
{

}
