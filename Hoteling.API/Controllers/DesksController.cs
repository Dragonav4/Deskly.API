using Hoteling.Application.Interfaces;
using Hoteling.Application.Interfaces.IService;
using Hoteling.Application.Views.Common;
using Hoteling.Application.Views.Desk;
using Hoteling.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoteling.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Employee")]
public class DesksController(
    IDeskService service,
    ICrudMapper<Desk, DeskCreateView, DeskView> mapper,
    ILogger<DesksController> logger)
    : BaseCrudController<Desk, DeskCreateView, DeskView>(service, mapper, logger)
{
    /// <summary>
    /// Gets a paginated list of all desks
    /// </summary>
    /// <param name="skip">Number of items to skip for pagination</param>
    /// <param name="take">Number of items to take for pagination</param>
    /// <returns>Paginated list of desks with availability status</returns>
    [HttpGet]
    [AllowAnonymous]
    public override Task<ActionResult<ActionListView<DeskView>>> GetAllAsync(int? skip = null, int? take = null)
    {
        return base.GetAllAsync(skip, take);
    }
    /// <summary>
    /// Gets a specific desk by its ID
    /// </summary>
    /// <param name="id">Desk unique identifier</param>
    /// <returns>Desk details including availability and reservation info</returns>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public override Task<ActionResult<DeskView>> GetById(Guid id)
    {
        return base.GetById(id);
    }
}
