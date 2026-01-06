using Hoteling.Domain.Enums;
using Hoteling.Domain.Interfaces;

namespace Hoteling.Domain.Entities;

public class User : IHasId
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}