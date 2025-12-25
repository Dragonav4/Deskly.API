using Hoteling.Domain.Entities;
using Hoteling.Infastructure.Data;

namespace Hoteling.Infastructure.Repositories.Reservations;

public class ReservationRepository(AppDbContext dbContext) : CrudRepository<Reservation>(dbContext), IReservationRepository
{
    
}