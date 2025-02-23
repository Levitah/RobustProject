using Microsoft.Extensions.Logging;
using RobustProject.Services.Enums;
using RobustProject.Services.Models.Response;
using System.ComponentModel.DataAnnotations;

namespace RobustProject.Services.Validation;

public class ModelValidator : IModelValidator
{
    private readonly ILogger<ModelValidator> _logger;

    public ModelValidator(ILogger<ModelValidator> logger)
    {
        _logger = logger;
    }

    public IResponse<bool> Validate<T>(T model) where T : IValidatable
    {
        var response = new Response<bool>(true);
        var validationResults = new List<ValidationResult>();
        if (Validate(model, validationResults))
            return response;

        response.Content = false;
        response.AddError(ErrorCode.ModelValidationError, _logger, message: GenerateErrorMessage(validationResults));
        return response;
    }

    public IResponse<bool> Validate<T>(IEnumerable<T> models) where T : IValidatable
    {
        var response = new Response<bool>(true);
        var validationResults = new List<ValidationResult>();

        foreach (var model in models)
            Validate(model, validationResults);

        if (!validationResults.Any())
            return response;

        response.Content = false;
        response.AddError(ErrorCode.ModelValidationError, _logger, message: GenerateErrorMessage(validationResults));
        return response;
    }

    private bool Validate<T>(T model, List<ValidationResult> validationResults) where T : IValidatable =>
        Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

    private string GenerateErrorMessage(List<ValidationResult> validationResults) => string.Join(" ", validationResults.Select(x => x.ErrorMessage!.ToString()));
}