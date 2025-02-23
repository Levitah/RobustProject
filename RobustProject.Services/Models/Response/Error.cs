using RobustProject.Services.Enums;
using RobustProject.Services.Extensions;
using System.Net;

namespace RobustProject.Services.Models.Response;

public class Error
{
    public HttpStatusCode HttpStatusCode { get; set; }
    public string? Message { get; set; }
    public string? Description { get; set; }
    public string? CallerMemberName { get; set; }
    
    public Error(ErrorCode errorCode, string? message, string callerMemeberName)
    {
        HttpStatusCode = errorCode.GetHttpStatusCode();
        Message = message;
        Description = errorCode.GetDescription();
        CallerMemberName = callerMemeberName;
    }

    public override string ToString()
    {
        return $"{Description} {Message} from {CallerMemberName}";
    }
}