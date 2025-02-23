using RobustProject.Services.Models.Response;

namespace RobustProject.Services.Repositories;

public interface IRepository<TModel>
{
    Task<IResponse<TModel>> AddAsync(TModel model, CancellationToken cancellationToken = default);

    Task<IResponse<bool>> AddAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default);

    Task<IResponse<IEnumerable<TModel>>> GetPagedAsync(int skip, int take, CancellationToken cancellationToken = default);
}