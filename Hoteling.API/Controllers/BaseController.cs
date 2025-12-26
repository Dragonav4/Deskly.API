using Hoteling.Application.Interfaces;
using Hoteling.Application.Interfaces.IService;
using Hoteling.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoteling.API.Controllers;

//[Authorize]
public abstract class BaseCrudController<T, TCreateView, TView>
    (IService<T> crudService, ICrudMapper<T, TCreateView, TView> mapper, ILogger logger)
    : ControllerBase
    where T : class, IHasId
    where TView : TCreateView, IHasId
{
    protected readonly ILogger Logger = logger;

    [HttpPost]
    public virtual async Task<ActionResult<TView>> CreateAsync([FromBody] TCreateView createDto)
    {
        Logger.LogInformation("Creating new {EntityName}", typeof(T).Name);
        var input = mapper.MapCreateDtoToDomain(createDto);
        var domain = await crudService.CreateAsync(input);
        OnItemCreated(domain);
        var result = mapper.MapDomainToView(domain);
        Logger.LogInformation("{EntityName} created with ID {Id}", typeof(T).Name, domain.Id);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public virtual async Task<ActionResult<TView>> UpdateAsync([FromRoute] Guid id, [FromBody] TView updateDto)
    {
        //updateDto.Id = id; // Force ID from route to avoid mismatch

        Logger.LogInformation("Updating {EntityName} with ID {Id}", typeof(T).Name, id);

        var input = mapper.MapViewToDomain(id, updateDto);
        var domain = await crudService.UpdateAsync(input);

        if (domain == null)
        {
            Logger.LogWarning("{EntityName} with ID {Id} not found for update", typeof(T).Name, id);
            return NotFound();
        }

        OnItemUpdated(domain);
        var result = mapper.MapDomainToView(domain);
        Logger.LogInformation("{EntityName} with ID {Id} updated successfully", typeof(T).Name, id);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public virtual async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
    {
        Logger.LogInformation("Deleting {EntityName} with ID {Id}", typeof(T).Name, id);
        var success = await crudService.DeleteAsync(id);
        if (!success)
        {
            Logger.LogWarning("{EntityName} with ID {Id} not found for deletion", typeof(T).Name, id);
            return NotFound();
        }

        Logger.LogInformation("{EntityName} with ID {Id} deleted successfully", typeof(T).Name, id);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public virtual async Task<ActionResult<TView>> GetById([FromRoute] Guid id)
    {
        Logger.LogInformation("Getting {EntityName} with ID {Id}", typeof(T).Name, id);
        var domain = await crudService.GetByIdAsync(id);
        if (domain == null)
        {
            Logger.LogWarning("{EntityName} with ID {Id} not found", typeof(T).Name, id);
            return NotFound();
        }

        var result = mapper.MapDomainToView(domain);
        return Ok(result);
    }

    [HttpGet]
    public virtual async Task<ActionResult<List<TView>>> GetAll()
    {
        Logger.LogInformation("Getting all {EntityName}s", typeof(T).Name);
        var domain = await crudService.GetAllAsync();
        var result = domain
            .Select(item => mapper.MapDomainToView(item))
            .ToList();
        return Ok(result);
    }

    protected virtual void OnItemCreated(T item)
    {
    }

    protected virtual void OnItemUpdated(T item)
    {
    }
}
