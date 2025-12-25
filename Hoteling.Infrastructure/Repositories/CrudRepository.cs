using Hoteling.Application.Interfaces.IRepository;
using Hoteling.Domain.Interfaces;
using Hoteling.Infastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hoteling.Infastructure.Repositories;

public class CrudRepository<T>(AppDbContext dbContext) : ICrudRepository<T> where T : class, IHasId
{
    protected readonly AppDbContext _dbContext = dbContext;
    protected readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken: cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;
        if (skip.HasValue) query = query.Skip(skip.Value);
        if (take.HasValue) query = query.Take(take.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task<T?> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var existing = await _dbSet.FindAsync([entity.Id], cancellationToken: cancellationToken);
        if (existing == null) return null;

        _dbContext.Entry(existing).CurrentValues.SetValues(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync([id], cancellationToken: cancellationToken);
        if (entity == null) return false;

        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
