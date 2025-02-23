using RobustProject.Services.Attributes;
using System.Net;

namespace RobustProject.Services.Enums;

public enum ErrorCode
{
    [ErrorCode(HttpStatusCode.InternalServerError, "General error")]
    GeneralError = 0,

    [ErrorCode(HttpStatusCode.BadRequest, "Model validation error")]
    ModelValidationError = 1,

    [ErrorCode(HttpStatusCode.InternalServerError, "Data persistence error")]
    DataPersistenceError = 2,

    [ErrorCode(HttpStatusCode.NotFound, "Not found error")]
    NotFound = 3,
}