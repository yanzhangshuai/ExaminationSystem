using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AccountController: ControllerBase
{

    [HttpPost]
    public Task Login([FromBody] LoginInput input)
    {
        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaims([
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString("N")),
            new Claim(ClaimTypes.Name, input.UserName),
            new Claim(ClaimTypes.Role, "Admin")
        ]);


        var principal = new ClaimsPrincipal(identity);

        // 登录
        var properties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(60),
            AllowRefresh = true
        };
        return HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

    }

    [HttpPost]
    public Task Logout()
    {
        return HttpContext.SignOutAsync();

    }
}

public class LoginInput
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
