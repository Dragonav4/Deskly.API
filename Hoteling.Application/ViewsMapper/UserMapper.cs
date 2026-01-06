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
            Items = domains.Select(d => MapDomainToView(d, user)).ToList(),
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
            UserName = createDto.UserName,
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
            UserName = viewDto.UserName,
            Role = viewDto.Role
        };
    }

    public UserView MapDomainToView(User viewDto, ClaimsPrincipal user)
    {
        return new UserView
        {
            Id = viewDto.Id,
            Email = viewDto.Email,
            UserName = viewDto.UserName,
            Role = viewDto.Role
        };
    }
}
