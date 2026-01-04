namespace Hoteling.Application.Exceptions;

public class DeskOccupiedException(string message)
    : Exception(message)
{
    public string ErrorCode { get; } = "DESK_OCCUPIED";
}
