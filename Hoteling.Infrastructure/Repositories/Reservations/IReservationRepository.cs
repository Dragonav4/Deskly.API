using Hoteling.Application.Interfaces.IRepository;
using Hoteling.Domain.Entities;

namespace Hoteling.Infastructure.Repositories.Reservations;

public interface IReservationRepository : ICrudRepository<Reservation>
{
    
}