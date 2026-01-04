using Hoteling.Application.Interfaces.IRepository;
using Hoteling.Domain.Entities;
using Hoteling.Infastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hoteling.Infastructure.Repositories.Desks;

public class DeskRepository(AppDbContext dbContext) : CrudRepository<Desk>(dbContext), IDeskRepository
{
}
