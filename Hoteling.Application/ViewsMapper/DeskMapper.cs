using System.Security.Claims;
using Hoteling.Application.Interfaces;
using Hoteling.Application.Interfaces.IRepository;
using Hoteling.Application.Views.Common;
using Hoteling.Application.Views.Desk;
using Hoteling.Domain.Auth;
using Hoteling.Domain.Entities;
using Hoteling.Domain.Enums;

namespace Hoteling.Application.ViewsMapper;

public class DeskMapper(
    IReservationRepository reservationRepository)
    : ICrudMapper<Desk, DeskCreateView, DeskView>
{
    public ActionListView<DeskView> MapDomainModelsToListView(IEnumerable<Desk> domainModels, int totalCount, ClaimsPrincipal user)
    {
        var items = domainModels.Select(item => MapDomainToView(item, user)).ToList();
        return new ActionListView<DeskView>
        {
            Items = items,
            TotalCount = totalCount,
            Actions = GetListActions(user)
        };
    }

    private static int GetListActions(ClaimsPrincipal user)
    {
        var isAdmin = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value == UserRole.Admin.ToString();
        return AuthClaims.ViewAction
               | (isAdmin ? AuthClaims.EditAction | AuthClaims.DeleteAction : 0);
    }

    public Desk MapCreateDtoToDomain(DeskCreateView createDto)
    {
        return new Desk
        {
            Id = Guid.NewGuid(),
            DeskNumber = createDto.DeskNumber,
            Floor = createDto.Floor,
            HasDualMonitor = createDto.HasDualMonitor,
            IsStandingDesk = createDto.IsStandingDesk
        };
    }

    public Desk MapViewToDomain(Guid id, DeskView viewDto)
    {
        viewDto.Id = id;
        return new Desk
        {
            Id = id,
            DeskNumber = viewDto.DeskNumber,
            Floor = viewDto.Floor,
            HasDualMonitor = viewDto.HasDualMonitor,
            IsStandingDesk = viewDto.IsStandingDesk
        };
    }

    public DeskView MapDomainToView(Desk domain, ClaimsPrincipal user)
    {
        var reservation = reservationRepository
            .GetByDateAndDeskAsync(DateTime.Today, domain.Id)
            .Result
            .FirstOrDefault();

        var isUser = user.Identity?.IsAuthenticated ?? false;
        var result = new DeskView
        {
            Id = domain.Id,
            DeskNumber = domain.DeskNumber,
            Floor = domain.Floor,
            HasDualMonitor = domain.HasDualMonitor,
            IsStandingDesk = domain.IsStandingDesk,
            IsOccupied = reservation != null,
            Actions = GetItemActions(user),
            ReservedByUserName = isUser ? reservation?.User?.UserName : null,
            ReservationDate = isUser ? reservation?.ReservationDate : null,
        };

        return result;
    }

    private static int GetItemActions(ClaimsPrincipal user)
    {
        return GetListActions(user);
    }
}
