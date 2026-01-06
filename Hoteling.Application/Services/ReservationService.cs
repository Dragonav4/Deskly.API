using Hoteling.Application.Exceptions;
using Hoteling.Application.Interfaces.IRepository;
using Hoteling.Application.Interfaces.IService;
using Hoteling.Domain.Entities;

namespace Hoteling.Application.Services;

public class ReservationService(IReservationRepository repository)
    : CrudService<Reservation>(repository), IReservationService
{
    public override async Task<Reservation> CreateAsync(Reservation model, CancellationToken cancellationToken = default)
    {
        var existingReservations = await repository.GetByDateAndDeskAsync(model.ReservationDate, model.DeskId, cancellationToken);

        if (existingReservations.Any())
        {
            throw new ConflictException(
                $"Desk is already occupied on {model.ReservationDate:yyyy-MM-dd}"
            );
        }

        return await base.CreateAsync(model, cancellationToken);
    }
}
