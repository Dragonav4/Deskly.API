using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace Hoteling.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet("login")]
    public IActionResult Login()
    {
        return Challenge(
            new AuthenticationProperties
            {
                RedirectUri = "/"
            }, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        return SignOut(
            new AuthenticationProperties
            {
                RedirectUri = "/"
            }, CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
