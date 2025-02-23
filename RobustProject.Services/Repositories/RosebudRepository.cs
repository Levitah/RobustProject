using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RobustProject.Services.Entities;
using RobustProject.Services.Models;
using RobustProject.Services.Models.Response;
using RobustProject.Services.Options;
using RobustProject.Services.Repositories.NoDatabase;
using RobustProject.Services.Specifications;

namespace RobustProject.Services.Repositories;

public class RosebudRepository : Repository<RosebudModel, Rosebud, RosebudRepository>, IRosebudRepository
{
    private readonly MongoDbOptions _mongoDbOptions;

    protected RosebudRepository(IMapper mapper, IEntityRepository<Rosebud> entityRepository, IUnitOfWork unitOfWork, ILogger<RosebudRepository> logger, IOptions<MongoDbOptions> mongoDbOptions) : base(mapper, entityRepository, unitOfWork, logger)
    {
        _mongoDbOptions = mongoDbOptions.Value;
    }

    public async Task<IResponse<bool>> DeleteByFamilyAsync(string family, CancellationToken cancellationToken = default)
    {
        var spec = new RosebudContainsFamilySpecification(family);
        return await DeleteAsync(spec, cancellationToken);
    }

    public async Task<IResponse<RosebudModel>> GetByFamilyAsync(string family, CancellationToken cancellationToken = default)
    {
        var spec = new RosebudContainsFamilySpecification(family);
        return await FindOneBySpecificationAsync(spec);
    }

    public async Task<IResponse<bool>> UpdateAsync(RosebudModel model, CancellationToken cancellationToken = default)
    {
        var spec = new RosebudContainsFamilySpecification(model.Family);
        return await UpdateAsync(model, spec, cancellationToken);
    }
}