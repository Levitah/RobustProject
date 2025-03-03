using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using RobustProject.Functions.Extensions;
using RobustProject.Services.Models;
using RobustProject.Services.Services.RosebudService;
using RobustProject.Services.Validation;
using System.Threading;
using System.Threading.Tasks;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace RobustProject.Functions.Rosebud;

public class Create
{
    private readonly IRosebudService _rosebudService;
    private readonly IModelValidator _modelValidator;

    public Create(IRosebudService rosebudService, IModelValidator modelValidator)
    {
        _rosebudService = rosebudService;
        _modelValidator = modelValidator;
    }

    [Function("CreateRosebud")]
    public async Task<IActionResult> CreateRosebud([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "2024-09-04/CreateRosebud")][FromBody] RosebudModel rosebudModel, CancellationToken cancellationToken)
    {
        var validation = _modelValidator.Validate(rosebudModel);
        if (validation.HasError) return validation.GetActionResultFromResponse();

        var response = await _rosebudService.AddAsync(rosebudModel, cancellationToken);
        return response.GetActionResultFromResponse();
    }
}