﻿using RobustProject.Services.Attributes;
using System.Net;

namespace RobustProject.Services.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var errorCodeAttribute = GetErrorCodeAttribute(value);
        return errorCodeAttribute.Description;
    }

    public static HttpStatusCode GetHttpStatusCode(this Enum value)
    {
        var errorCodeAttribute = GetErrorCodeAttribute(value);
        return errorCodeAttribute.HttpStatusCode;
    }

    private static ErrorCodeAttribute GetErrorCodeAttribute(Enum value)
    {
        return (value.GetType()
            .GetField(value.ToString())?
            .GetCustomAttributes(typeof(ErrorCodeAttribute), false)
            .FirstOrDefault() as ErrorCodeAttribute)!;
    }
}