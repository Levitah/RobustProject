using RobustProject.Services.Models.Response;

namespace RobustProject.Services.Validation;

public interface IModelValidator
{
    IResponse<bool> Validate<T>(T model) where T : IValidatable;
    IResponse<bool> Validate<T>(IEnumerable<T> model) where T : IValidatable;
}