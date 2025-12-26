using System.ComponentModel.DataAnnotations;
using Hoteling.Domain.Interfaces;

namespace Hoteling.Application.Views.Desk;

public class DeskCreateView
{
    [Required]
    public string DeskNumber { get; set; } = string.Empty;
    public bool HasDualMonitor { get; set; }
    public bool IsStandingDesk { get; set; }
    public int Floor { get; set; }
}

public class DeskView : DeskCreateView, IHasId
{
    public Guid Id { get; set; }
}
