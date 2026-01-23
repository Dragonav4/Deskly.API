using Hoteling.Application.Interfaces.IRepository;
using Hoteling.Domain.Entities;
using Hoteling.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hoteling.Infrastructure.Repositories.Desks;

public class DeskRepository(AppDbContext dbContext) : CrudRepository<Desk>(dbContext), IDeskRepository
{
}
