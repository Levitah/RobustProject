using RobustProject.Services.Models.Response;

namespace RobustProject.Services.Repositories;

public interface IRepository<TModel, TId>
{
    Task<IResponse<TModel>> AddAsync(TModel model, CancellationToken cancellationToken = default);
    Task<IResponse<bool>> AddAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default);
    Task<IResponse<IEnumerable<TModel>>> GetPagedAsync(int skip, int take, CancellationToken cancellationToken = default);
    Task<IResponse<TModel>> GetAsync(TId id, CancellationToken cancellationToken = default);
    Task<IResponse<IEnumerable<TModel>>> GetAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);
    Task<IResponse<bool>> DeleteAsync(TId id, CancellationToken cancellationToken = default);
    Task<IResponse<bool>> DeleteAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);
    Task<IResponse<TModel>> UpdateAsync(TModel model, CancellationToken cancellationToken = default);
    Task<IResponse<bool>> UpdateAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default);
}