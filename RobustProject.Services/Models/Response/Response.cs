using Microsoft.Extensions.Logging;
using RobustProject.Services.Enums;
using System.Runtime.CompilerServices;

namespace RobustProject.Services.Models.Response;

public class Response<T> : IResponse<T>
{
    private readonly List<Error> _errorList = new();

    public T? Content { get; set; }

    public IEnumerable<Error> Errors => _errorList;

    public bool HasError => _errorList.Any();

    public Response(T content)
    {
        Content = content;
    }

    public Response()
    {

    }

    public Response<T> AddError(ErrorCode errorCode, ILogger logger, Exception? ex = null, string message = "", [CallerMemberName] string callerMemberName = "")
    {
        var error = new Error(errorCode, ex?.Message ?? message, callerMemberName);
        _errorList.Add(error);
        logger.LogError(error.ToString());

        return this;
    }

    public Response<T> AddErrors<TK>(IResponse<TK>? response)
    {
        if (response != null)
        {
            _errorList.AddRange(response.Errors);
        }

        return this;
    }
}