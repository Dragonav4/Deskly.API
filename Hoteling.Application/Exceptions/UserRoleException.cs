namespace Hoteling.Application.Exceptions;

public class UserRoleException(string message)
    : Exception(message)
{
    public string ErrorCode { get; } = "USER ALREADY HAS ROLE";
}
