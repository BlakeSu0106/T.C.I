using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Telligent.Consumer.Identity.Application.AppServices;

namespace Telligent.Consumer.Identity.Server.Controllers;

[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IdentityAppService _identityAppService;

    public AuthorizationController(IdentityAppService identityAppService)
    {
        _identityAppService = identityAppService;
    }

    [HttpPost("~/connect/token"), Produces("application/json")]
    public async Task<IActionResult> ExchangeAsync()
    {
        return await _identityAppService.ExchangeAsync(HttpContext.GetOpenIddictServerRequest());
    }

    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> AuthorizeAsync()
    {
        return await _identityAppService.AuthorizeAsync(HttpContext.GetOpenIddictClientRequest());
    }

    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
    [HttpGet("~/connect/userinfo")]
    public async Task<IActionResult> GetUserInfoAsync()
    {
        var claimsPrincipal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;

        return Ok(new
        {
            Name = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Subject),
            Occupation = "Developer",
            Age = 43
        });
    }
}