using Hoteling.Domain.Entities;

namespace Hoteling.Application.Interfaces.IRepository;

public interface IReservationRepository : ICrudRepository<Reservation>
{
    Task<IReadOnlyList<Reservation>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default);
}
