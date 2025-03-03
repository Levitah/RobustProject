using Microsoft.EntityFrameworkCore;
using RobustProject.Services.Entities;
using RobustProject.Services.Specifications;

namespace RobustProject.Services.Repositories.SqlServer;

public class EntityRepository<TEntity, TId> : IEntityRepository<TEntity, TId> where TEntity : Entity<TId>, IEntity<TId>
{
    private readonly DbContext _dbContext;

    public EntityRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Load(TEntity entity)
    {
        foreach (var reference in _dbContext.Entry(entity!).References)
        {
            reference.Load();
        }
    }

    public void Load(TEntity entity, string propertyName)
    {
        _dbContext.Entry(entity!).Reference(propertyName).Load();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().Skip(skip).Take(take).ToListAsync(cancellationToken);
    }

    public virtual TEntity Add(TEntity entity)
    {
        var entry = _dbContext.Set<TEntity>().Add(entity);
        return entry.Entity;
    }

    public virtual void Add(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().AddRange(entities);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        return entry.Entity;
    }

    public virtual async Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
    }

    public virtual void Remove(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
    }

    public virtual void Remove(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().RemoveRange(entities);
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellationTEntityToken = default)
    {
        return await _dbContext.Set<TEntity>().CountAsync(cancellationTEntityToken);
    }

    public virtual IQueryable<TEntity> Where(ISpecification<TEntity> specification)
    {
        return _dbContext.Set<TEntity>().Where(specification.Predicate);
    }

    public virtual async Task<bool> AnyAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationTEntityToken = default)
    {
        return await _dbContext.Set<TEntity>().AnyAsync(specification.Predicate, cancellationTEntityToken);
    }

    public async Task<bool> AllAsync(ISpecification<TEntity> specification, CancellationToken cancellationTEntityToken = default)
    {
        return await _dbContext.Set<TEntity>().AllAsync(specification.Predicate, cancellationTEntityToken);
    }

    public virtual TEntity? FindById(TId id)
    {
        return _dbContext.Set<TEntity>().FirstOrDefault(x => x.Id!.Equals(id));
    }

    public virtual async Task<TEntity?> FindByIdAsync(TId id,
        CancellationToken cancellationTEntityToken = default)
    {
        return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationTEntityToken);
    }

    public virtual async Task<IEnumerable<TEntity?>> FindByIdAsync(IEnumerable<TId> ids,
        CancellationToken cancellationTEntityToken = default)
    {
        return await _dbContext.Set<TEntity>().Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken: cancellationTEntityToken);
    }

    public virtual async Task RemoveAsync(TId id, CancellationToken cancellationTEntityToken = default)
    {
        var entity = await FindByIdAsync(id, cancellationTEntityToken);
        if (entity == null)
        {
            throw new ArgumentException(nameof(id));
        }
        Remove(entity);
    }


    public virtual async Task RemoveAsync(IEnumerable<TId> ids, CancellationToken cancellationTEntityToken = default)
    {
        var entities = await FindByIdAsync(ids, cancellationTEntityToken);
        if (entities == null)
        {
            throw new ArgumentException(nameof(ids));
        }

        Remove(entities!);
    }
}