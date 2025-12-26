using Hoteling.Application.Interfaces;
using Hoteling.Application.Views.Reservation;
using Hoteling.Application.Views.Desk;
using Hoteling.Application.Views.User;
using Hoteling.Domain.Entities;

namespace Hoteling.Application.ViewsMapper;

public class ReservationMapper(
    ICrudMapper<Desk, DeskCreateView, DeskView> deskMapper,
    ICrudMapper<User, UserCreateView, UserView> userMapper)
    : ICrudMapper<Reservation, ReservationCreateView, ReservationView>
{
    public Reservation MapCreateDtoToDomain(ReservationCreateView createDto)
    {
        return new Reservation
        {
            Id = Guid.NewGuid(),
            DeskId = createDto.DeskId,
            UserId = createDto.UserId,
            ReservationDate = createDto.ReservationDate,
            CreatedAt = DateTime.Now
        };
    }

    public Reservation MapViewToDomain(Guid id, ReservationView viewDto)
    {
        viewDto.Id = id;
        return new Reservation
        {
            Id = id,
            DeskId = viewDto.DeskId,
            UserId = viewDto.UserId,
            ReservationDate = viewDto.ReservationDate,
            CreatedAt = viewDto.CreatedAt
        };
    }

    public ReservationView MapDomainToView(Reservation viewDto)
    {
        return new ReservationView
        {
            Id = viewDto.Id,
            DeskId = viewDto.DeskId,
            UserId = viewDto.UserId,
            ReservationDate = viewDto.ReservationDate,
            CreatedAt = viewDto.CreatedAt,
            Desk = viewDto.Desk != null ? deskMapper.MapDomainToView(viewDto.Desk) : null,
            User = viewDto.User != null ? userMapper.MapDomainToView(viewDto.User) : null
        };
    }
}
