using Hoteling.API.Exceptions;
using Hoteling.Application.Exceptions;
using Hoteling.Application.Interfaces;
using Hoteling.Application.Interfaces.IService;
using Hoteling.Application.Views.Common;
using Hoteling.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hoteling.API.Controllers;

//[Authorize]
public abstract class BaseCrudController<T, TCreateView, TView>
    (IService<T> service, ICrudMapper<T, TCreateView, TView> mapper, ILogger logger)
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
        var domain = await service.CreateAsync(input);
        OnItemCreated(domain);
        var result = mapper.MapDomainToView(domain, User);
        Logger.LogInformation("{EntityName} created with ID {Id}", typeof(T).Name, domain.Id);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public virtual async Task<ActionResult<TView>> UpdateAsync([FromRoute] Guid id, [FromBody] TView updateDto)
    {

        Logger.LogInformation("Updating {EntityName} with ID {Id}", typeof(T).Name, id);

        var input = mapper.MapViewToDomain(id, updateDto);
        var domain = await service.UpdateAsync(input);

        if (domain == null)
        {
            throw new NotFoundException($"{typeof(T).Name} with ID {id} not found for update");
        }

        OnItemUpdated(domain);
        var result = mapper.MapDomainToView(domain, User);
        Logger.LogInformation("{EntityName} with ID {Id} updated successfully", typeof(T).Name, id);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public virtual async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
    {
        Logger.LogInformation("Deleting {EntityName} with ID {Id}", typeof(T).Name, id);
        var success = await service.DeleteAsync(id);
        if (!success)
        {
            throw new NotFoundException($"{typeof(T).Name} with ID {id} not found for deletion");
        }

        Logger.LogInformation("{EntityName} with ID {Id} deleted successfully", typeof(T).Name, id);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public virtual async Task<ActionResult<TView>> GetById([FromRoute] Guid id)
    {
        Logger.LogInformation("Getting {EntityName} with ID {Id}", typeof(T).Name, id);
        var domain = await service.GetByIdAsync(id);
        if (domain == null)
        {
            throw new NotFoundException($"{typeof(T).Name} with ID {id} not found");
        }

        var result = mapper.MapDomainToView(domain, User);
        return Ok(result);
    }

    [HttpGet]
    public virtual async Task<ActionResult<ActionListView<TView>>> GetAllAsync(int? skip = null, int? take = null)
    {
        Logger.LogInformation("Getting all {EntityName}s", typeof(T).Name);
        var (items, totalCount) = await service.GetAllAsync(skip, take);

        var result = mapper.MapDomainModelsToListView(items, totalCount, User);
        return Ok(result);
    }

    protected virtual void OnItemCreated(T item)
    {
    }

    protected virtual void OnItemUpdated(T item)
    {
    }
}
