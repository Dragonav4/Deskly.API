using System.Security.Claims;
using Hoteling.Application.Interfaces;
using Hoteling.Application.Interfaces.IService;
using Hoteling.Application.Views.Common;
using Hoteling.Application.Views.Reservation;
using Hoteling.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoteling.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Employee")]
public class ReservationsController(
    IService<Reservation> reservationService,
    ICrudMapper<Reservation, ReservationCreateView, ReservationView> mapper,
    ILogger<ReservationsController> logger)
    : BaseCrudController<Reservation, ReservationCreateView, ReservationView>(reservationService, mapper, logger)
{
    public override async Task<ActionResult<ReservationView>> CreateAsync(ReservationCreateView createDto)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        logger.LogInformation("Attempting to create reservation. User ID from claims: '{UserIdString}'", userIdString);

        if (!Guid.TryParse(userIdString, out var userGuid))
        {
            logger.LogError("Failed to parse User ID claim '{UserId}' as GUID for user {UserName}", userIdString,
                User.Identity?.Name);
            return BadRequest(userGuid);
        }

        createDto.UserId = userGuid;
        var result = await base.CreateAsync(createDto);
        return result;
    }


    public override async Task<ActionResult<ReservationView>> UpdateAsync(Guid id, ReservationView updateDto)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        logger.LogInformation("Attempting to update reservation. User ID from claims: '{UserIdString}'", userIdString);

        if (!Guid.TryParse(userIdString, out var userGuid))
        {
            logger.LogError("Failed to parse User ID claim '{UserId}' as GUID for user {UserName}", userIdString,
                User.Identity?.Name);
            return BadRequest(userGuid);
        }

        updateDto.UserId = userGuid;
        return await base.UpdateAsync(id, updateDto);
    }

    [HttpGet]
    [AllowAnonymous]
    public override Task<ActionResult<ActionListView<ReservationView>>> GetAllAsync(int? skip = null, int? take = null)
    {
        return base.GetAllAsync(skip, take);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public override Task<ActionResult<ReservationView>> GetById(Guid id)
    {
        return base.GetById(id);
    }
}
