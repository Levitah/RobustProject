using Microsoft.AspNetCore.Mvc;
using RobustProject.Services.Models.Response;
using System;
using System.Linq;
using System.Net;

namespace RobustProject.Functions.Extensions;

public static class ResponseExtensions
{
    public static IActionResult GetActionResultFromResponse<T>(
        this IResponse<T> response, HttpStatusCode successfulHttpStatusCode = HttpStatusCode.OK)
    {
        if (response.HasError)
        {
            var errorContent = response.Errors.Select(x => x.ToString());
            var statusCode = response.Errors.Count() == 1 ? response.Errors.First().HttpStatusCode : HttpStatusCode.InternalServerError;
            return CreateObjectResult(statusCode, string.Join(Environment.NewLine, errorContent));
        }

        if (response.Content is not null)
        {
            return CreateObjectResult(successfulHttpStatusCode, response.Content);
        }

        return new StatusCodeResult((int)successfulHttpStatusCode);
    }

    public static IActionResult GetActionResultFromBoolResponse(
        this IResponse<bool> response, HttpStatusCode successfulHttpStatusCode = HttpStatusCode.OK, HttpStatusCode failedHttpStatusCode = HttpStatusCode.BadRequest, string? errorMessage = null)
    {
        if (response.Content)
            return new StatusCodeResult((int)successfulHttpStatusCode);

        if (errorMessage == null)
            return new StatusCodeResult((int)failedHttpStatusCode);

        return CreateObjectResult(failedHttpStatusCode, errorMessage);
    }

    private static ObjectResult CreateObjectResult<T>(HttpStatusCode statusCode, T content)
    {
        return new ObjectResult(content)
        {
            StatusCode = (int)statusCode
        };
    }
}