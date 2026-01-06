using Hoteling.Application.Interfaces;
using Hoteling.Application.Interfaces.IService;
using Hoteling.Application.Views.User;
using Hoteling.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoteling.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize( Roles = "Admin")]
public class UsersController(IUserService service, ICrudMapper<User, UserCreateView, UserView> mapper, ILogger<UsersController> logger)
    : BaseCrudController<User, UserCreateView, UserView>(service, mapper, logger)
{
}
