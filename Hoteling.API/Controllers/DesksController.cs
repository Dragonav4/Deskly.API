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
    [HttpGet]
    [AllowAnonymous]
    public override Task<ActionResult<ActionListView<DeskView>>> GetAllAsync(int? skip = null, int? take = null)
    {
        return base.GetAllAsync(skip, take);
    }
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public override Task<ActionResult<DeskView>> GetById(Guid id)
    {
        return base.GetById(id);
    }
}
