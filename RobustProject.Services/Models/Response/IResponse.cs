namespace RobustProject.Services.Models.Response;

public interface IResponse<T>
{
    T? Content { get; }
    IEnumerable<Error> Errors { get; }
    bool HasError { get; }
}