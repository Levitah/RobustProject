using RobustProject.Services.Entities;
using RobustProject.Services.Specifications;

namespace RobustProject.Services.Repositories.NoDatabase;

public interface IEntityRepository<TEntity> where TEntity : IEntity
{
    void Load(TEntity entity);

    void Load(TEntity entity, string propertyName);

    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default);

    TEntity Add(TEntity entity);

    void Add(IEnumerable<TEntity> entities);

    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    void Delete(ISpecification<TEntity> specification);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    IQueryable<TEntity> Where(ISpecification<TEntity> specification);

    Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    Task<bool> AllAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
}