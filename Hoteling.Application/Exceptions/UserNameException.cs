namespace Hoteling.Application.Exceptions;

public class UserNameException(string message)
    : Exception(message)
{
    public string ErrorCode { get; } = "USER ALREADY HAS THIS NAME";
}
