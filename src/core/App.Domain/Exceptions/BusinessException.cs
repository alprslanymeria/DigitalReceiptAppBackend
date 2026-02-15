using System.Net;

namespace App.Domain.Exceptions;

/// <summary>
/// EXCEPTION THROWN WHEN A BUSINESS RULE IS VIOLATED.
/// </summary>
public sealed class BusinessException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : AppException(message, statusCode);
