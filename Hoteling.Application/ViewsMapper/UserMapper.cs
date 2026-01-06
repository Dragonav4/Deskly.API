using System.Security.Claims;
using Hoteling.Application.Interfaces;
using Hoteling.Application.Views.Common;
using Hoteling.Application.Views.User;
using Hoteling.Domain.Auth;
using Hoteling.Domain.Entities;
using Hoteling.Domain.Enums;

namespace Hoteling.Application.ViewsMapper;

public class UserMapper : ICrudMapper<User, UserCreateView, UserView>
{
    private static int GetListActions(ClaimsPrincipal user)
    {
        var isAdmin = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value == UserRole.Admin.ToString();
        return AuthClaims.ViewAction
               | (isAdmin ? AuthClaims.EditAction | AuthClaims.DeleteAction : 0);
    }

    public ActionListView<UserView> MapDomainModelsToListView(IEnumerable<User> domains, int totalCount, ClaimsPrincipal user)
    {
        return new ActionListView<UserView>
        {
            Items = domains.Select(d => MapDomainToView(d, user)).OrderByDescending(r => r.Role).ToList(),
            TotalCount = totalCount,
            Actions = GetListActions(user),
        };
    }

    public User MapCreateDtoToDomain(UserCreateView createDto)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = createDto.Email,
            UserName = createDto.Username,
            Role = createDto.Role
        };
    }

    public User MapViewToDomain(Guid id, UserView viewDto)
    {
        viewDto.Id = id;
        return new User
        {
            Id = id,
            Email = viewDto.Email,
            UserName = viewDto.Username,
            Role = viewDto.Role
        };
    }

    public UserView MapDomainToView(User domain, ClaimsPrincipal user)
    {
        return new UserView
        {
            Id = domain.Id,
            Email = domain.Email,
            Username = domain.UserName,
            Role = domain.Role,
            Actions = GetItemActions(user)
        };
    }

    private static int GetItemActions(ClaimsPrincipal user)
    {
        return GetListActions(user);
    }
}
