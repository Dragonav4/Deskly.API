using Hoteling.Application.Interfaces;
using Hoteling.Application.Interfaces.IRepository;
using Hoteling.Application.Interfaces.IService;
using Hoteling.Application.Views.Desk;
using Hoteling.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Hoteling.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DesksController(
    IService<Desk> crudService,
    ICrudMapper<Desk, DeskCreateView, DeskView> mapper,
    IReservationRepository reservationRepository,
    ILogger<DesksController> logger)
    : BaseCrudController<Desk, DeskCreateView, DeskView>(crudService, mapper, logger)
{
    [HttpGet("map")]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public async Task<IActionResult> GetMap([FromQuery] DateTime? date, CancellationToken cancellationToken)
    {
        var targetDate = date ?? DateTime.Today;
        var desks = await crudService.GetAllAsync(cancellationToken: cancellationToken);
        var reservations = await reservationRepository.GetByDateAsync(targetDate, cancellationToken);

        var isGuest = !User.Identity?.IsAuthenticated ?? true
                      || User.IsInRole(Domain.Enums.UserRole.Guest.ToString());

        var result = desks.Select(desk =>
        {
            var reservation = reservations.FirstOrDefault(r => r.DeskId == desk.Id);
            var view = new DeskMapView
            {
                Id = desk.Id,
                DeskNumber = desk.DeskNumber,
                HasDualMonitor = desk.HasDualMonitor,
                IsStandingDesk = desk.IsStandingDesk,
                Floor = desk.Floor,
                IsOccupied = reservation != null
            };

            if (!isGuest && reservation != null)
            {
                view.ReservedByUserId = reservation.UserId;
                view.ReservedByUserName = reservation.User?.UserName;
                view.ReservationDate = reservation.ReservationDate;
            }

            return view;
        }).ToList();

        return Ok(result);
    }
}
