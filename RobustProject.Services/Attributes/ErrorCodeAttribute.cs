using System.ComponentModel;
using System.Net;

namespace RobustProject.Services.Attributes;

public class ErrorCodeAttribute : DescriptionAttribute
{
    private HttpStatusCode HttpStatusCodeValue { get; }

    public ErrorCodeAttribute(HttpStatusCode httpStatusCode, string description) : base(description)
    {
        HttpStatusCodeValue = httpStatusCode;
    }

    public virtual HttpStatusCode HttpStatusCode => HttpStatusCodeValue;
}