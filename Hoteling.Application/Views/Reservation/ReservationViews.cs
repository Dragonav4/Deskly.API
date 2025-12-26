using System.ComponentModel.DataAnnotations;
using Hoteling.Application.Views.Desk;
using Hoteling.Application.Views.User;
using Hoteling.Domain.Interfaces;

namespace Hoteling.Application.Views.Reservation;

public class ReservationCreateView
{
    [Required]
    public Guid DeskId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public DateTime ReservationDate { get; set; }
}

public class ReservationView : ReservationCreateView, IHasId
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public DeskView? Desk { get; set; }
    public UserView? User { get; set; }
}
