using RobustProject.Services.Entities;
using RobustProject.Services.Specifications;

namespace RobustProject.Services.Repositories.SqlServer;

public interface IEntityRepository<TEntity, TId> where TEntity : IEntity<TId>
{
    void Load(TEntity entity);
    void Load(TEntity entity, string propertyName);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default);
    TEntity Add(TEntity entity);
    void Add(IEnumerable<TEntity> entities);
    Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Remove(TEntity entity);
    void Remove(IEnumerable<TEntity> entities);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    IQueryable<TEntity> Where(ISpecification<TEntity> specification);
    Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<bool> AllAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    TEntity? FindById(TId id);
    Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity?>> FindByIdAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);
    Task RemoveAsync(TId id, CancellationToken cancellationToken = default);
    Task RemoveAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);
}