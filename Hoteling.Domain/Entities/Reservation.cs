using Hoteling.Domain.Enums;
using Hoteling.Domain.Interfaces;

namespace Hoteling.Domain.Entities;

public class Reservation : IHasId
{
    public Guid Id { get; set; }
    public Guid DeskId { get; set; }
    public Guid UserId { get; set; }

    public DateTime ReservationDate { get; set; } // todo change to ReservationStart + ReservationEnd / Duration
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Desk? Desk { get; set; }
    public User? User { get; set; }
}
