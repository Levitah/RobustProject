using RobustProject.Services.Models;
using RobustProject.Services.Models.Response;

namespace RobustProject.Services.Repositories;

public interface IRosebudRepository : IRepository<RosebudModel, int?>
{
    Task<IResponse<RosebudModel>> GetByFamilyAsync(string family, CancellationToken cancellationToken = default);
}