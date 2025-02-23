using RobustProject.Services.Models;
using RobustProject.Services.Models.Response;

namespace RobustProject.Services.Services.RosebudService;

public interface IRosebudService
{
    Task<IResponse<RosebudModel>> AddAsync(RosebudModel rosebudModel, CancellationToken cancellationToken = default);
}