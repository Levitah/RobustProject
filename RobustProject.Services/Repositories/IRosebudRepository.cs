using RobustProject.Services.Models;
using RobustProject.Services.Models.Response;

namespace RobustProject.Services.Repositories;

public interface IRosebudRepository : IRepository<RosebudModel>
{
    Task<IResponse<RosebudModel>> GetByFamilyAsync(string family, CancellationToken cancellationToken = default);

    Task<IResponse<bool>> DeleteByFamilyAsync(string family, CancellationToken cancellationToken = default);

    Task<IResponse<bool>> UpdateAsync(RosebudModel model, CancellationToken cancellationToken = default);
}