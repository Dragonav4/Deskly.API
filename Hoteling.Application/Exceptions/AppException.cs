using System.Net;

namespace Hoteling.Application.Exceptions;

public abstract class AppException(
    string message,
    string errorCode,
    HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
    public HttpStatusCode StatusCode { get; } = statusCode;
}

public class UserNotFoundException(string identifier)
    : AppException($"User with identifier '{identifier}' was not found.", "USER_NOT_FOUND", HttpStatusCode.NotFound);

public class InvalidPasswordException()
    : AppException("Invalid email or password.", "INVALID_CREDENTIALS", HttpStatusCode.Unauthorized);

public class EmailAlreadyExistsException(string email)
    : AppException($"Email '{email}' is already taken.", "EMAIL_EXISTS", HttpStatusCode.Conflict);

public class RefreshTokenInvalidException()
    : AppException("Refresh token is invalid or expired.", "INVALID_REFRESH_TOKEN", HttpStatusCode.Unauthorized);

public class InvalidCredentialsException()
    : AppException("Invalid email or password.", "INVALID_CREDENTIALS", HttpStatusCode.Unauthorized);

public class IdentityValidationException(IEnumerable<string> errors)
    : AppException(
        "Validation failed: " + string.Join("; ", errors),
        "IDENTITY_VALIDATION_ERROR",
        HttpStatusCode.BadRequest);

public class RoleAssignmentException(IEnumerable<string> errors)
    : AppException(
        "Failed to assign role: " + string.Join("; ", errors),
        "ROLE_ASSIGNMENT_ERROR",
        HttpStatusCode.InternalServerError);

public class NotFoundException(string message, string errorCode = "NOT_FOUND")
    : AppException(message, errorCode, HttpStatusCode.NotFound);

public class ConflictException(string message, string errorCode = "CONFLICT")
    : AppException(message, errorCode, HttpStatusCode.Conflict);

public class ForbiddenException(string message = "You are not allowed to access this resource", string errorCode = "FORBIDDEN")
    : AppException(message, errorCode, HttpStatusCode.Forbidden);
