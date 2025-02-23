using RobustProject.Services.Models;
using RobustProject.Services.Models.Response;
using RobustProject.Services.Repositories;

namespace RobustProject.Services.Services.RosebudService;

public class RosebudService : IRosebudService
{
    private readonly IRosebudRepository _rosebudRepository;

    public RosebudService(IRosebudRepository rosebudRepository)
    {
        _rosebudRepository = rosebudRepository;
    }

    public async Task<IResponse<RosebudModel>> AddAsync(RosebudModel rosebudModel, CancellationToken cancellationToken = default)
    {
        return await _rosebudRepository.AddAsync(rosebudModel, cancellationToken);
    }
}