using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RobustProject.Services.Entities;
using RobustProject.Services.Models;
using RobustProject.Services.Models.Response;
using RobustProject.Services.Options;
using RobustProject.Services.Repositories.SqlServer;
using RobustProject.Services.Specifications;

namespace RobustProject.Services.Repositories;

public class RosebudRepository : Repository<RosebudModel, int?, Rosebud, RosebudRepository>, IRosebudRepository
{
    private readonly SqlServerOptions _sqlServerOptions;

    public RosebudRepository(IMapper mapper, IEntityRepository<Rosebud, int?> entityRepository, IUnitOfWork unitOfWork, ILogger<RosebudRepository> logger, IOptions<SqlServerOptions> sqlServerOptions) : base(mapper, entityRepository, unitOfWork, logger)
    {
        _sqlServerOptions = sqlServerOptions.Value;
    }

    public async Task<IResponse<RosebudModel>> GetByFamilyAsync(string family, CancellationToken cancellationToken = default)
    {
        var spec = new RosebudContainsFamilySpecification(family);
        return await FindOneBySpecificationAsync(spec);
    }
}