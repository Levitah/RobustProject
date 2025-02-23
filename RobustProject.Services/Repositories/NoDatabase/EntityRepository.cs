using Microsoft.EntityFrameworkCore;
using RobustProject.Services.Entities;
using RobustProject.Services.Specifications;

namespace RobustProject.Services.Repositories.NoDatabase;

public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : Entity
{
    private readonly DbContext _dbContext;

    public EntityRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public TEntity Add(TEntity entity)
    {
        var entry = _dbContext.Set<TEntity>().Add(entity);
        return entry.Entity;
    }

    public void Add(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<IEnumerable<TEntity>>().AddRange(entities);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        return entry.Entity;
    }

    public async Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<IEnumerable<TEntity>>().AddAsync(entities, cancellationToken);
    }

    public async Task<bool> AllAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().AllAsync(specification.Predicate, cancellationToken);
    }

    public async Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().AnyAsync(specification.Predicate, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().CountAsync(cancellationToken);
    }

    public void Delete(ISpecification<TEntity> specification)
    {
        var entries = Where(specification);
        _dbContext.Set<TEntity>().RemoveRange(entries);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().Skip(skip).Take(take).ToListAsync(cancellationToken);
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

    public IQueryable<TEntity> Where(ISpecification<TEntity> specification)
    {
        return _dbContext.Set<TEntity>().Where(specification.Predicate);
    }
}