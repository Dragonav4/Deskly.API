using System.ComponentModel.DataAnnotations;
using Hoteling.Domain.Enums;
using Hoteling.Domain.Interfaces;

namespace Hoteling.Application.Views.User;

public class UserCreateView
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; }
}

public class UserView : UserCreateView, IHasId
{
    public Guid Id { get; set; }
    public int Actions { get; set; }
}
