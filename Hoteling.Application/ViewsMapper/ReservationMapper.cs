using System.Security.Claims;
using Hoteling.Application.Interfaces;
using Hoteling.Application.Views.Common;
using Hoteling.Application.Views.Reservation;
using Hoteling.Application.Views.Desk;
using Hoteling.Application.Views.User;
using Hoteling.Domain.Auth;
using Hoteling.Domain.Entities;
using Hoteling.Domain.Enums;

namespace Hoteling.Application.ViewsMapper;

public class ReservationMapper(
    ICrudMapper<Desk, DeskCreateView, DeskView> deskMapper,
    ICrudMapper<User, UserCreateView, UserView> userMapper)
    : ICrudMapper<Reservation, ReservationCreateView, ReservationView>
{
    public ActionListView<ReservationView> MapDomainModelsToListView(IEnumerable<Reservation> domains, int totalCount, ClaimsPrincipal user)
    {
        return new ActionListView<ReservationView>
        {
            Items = domains.Select(d => MapDomainToView(d, user)).OrderBy(d => d.ReservationDate).ToList(),
            TotalCount = totalCount,
            Actions = GetListActions(user),
        };
    }

    private static int GetListActions(ClaimsPrincipal user)
    {
        var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        if (role == UserRole.Employee.ToString() || role == UserRole.Admin.ToString())
        {
            return AuthClaims.ViewAction | AuthClaims.EditAction;
        }
        return AuthClaims.ViewAction;
    }

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

    public ReservationView MapDomainToView(Reservation domain, ClaimsPrincipal user)
    {
        var isAuthenticated = user.Identity?.IsAuthenticated ?? false;
        return new ReservationView
        {
            Id = domain.Id,
            DeskId = domain.DeskId,
            UserId = domain.UserId,
            ReservationDate = domain.ReservationDate,
            CreatedAt = domain.CreatedAt,
            Desk = domain.Desk != null ? deskMapper.MapDomainToView(domain.Desk, user) : null,
            User = (isAuthenticated && domain.User != null) ? userMapper.MapDomainToView(domain.User, user) : null,
            Actions = GetItemActions(domain, user)
        };
    }

    private static int GetItemActions(Reservation domain, ClaimsPrincipal user)
    {
        if (domain.User?.Id.ToString() == user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value)
        {
            return AuthClaims.ViewAction | AuthClaims.EditAction | AuthClaims.DeleteAction;
        }

        return GetListActions(user);
    }
}
