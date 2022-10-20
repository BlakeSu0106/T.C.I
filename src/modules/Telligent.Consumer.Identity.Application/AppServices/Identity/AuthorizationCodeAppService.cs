using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Telligent.Core.Infrastructure.Services;

namespace Telligent.Consumer.Identity.Application.AppServices.Identity;

public class AuthorizationCodeAppService : ControllerBase, IAppService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthorizationCodeAppService(IHttpContextAccessor httpContextAccessor)
    {
        _contextAccessor = httpContextAccessor;
    }

    public async Task<IActionResult> ExchangeAsync()
    {
        var claimsPrincipal =
            (await _contextAccessor.HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;
        return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    public async Task<IActionResult> AuthorizeAsync(OpenIddictRequest request)
    {
        // Retrieve the user principal stored in the authentication cookie.
        var result = await _contextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // If the user principal can't be extracted, redirect the user to the login page.
        if (!result.Succeeded)
        {
            var httpRequest = _contextAccessor.HttpContext.Request;

            return Challenge(
                authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = httpRequest.PathBase + httpRequest.Path + QueryString.Create(httpRequest.HasFormContentType ? httpRequest.Form.ToList() : httpRequest.Query.ToList())
                });
        }

        // Create a new claims principal
        var claims = new List<Claim>
            {
                // 'subject' claim which is required
                new Claim(OpenIddictConstants.Claims.Subject, result.Principal.Identity.Name),
                new Claim("some claim", "some value").SetDestinations(OpenIddictConstants.Destinations.AccessToken),
                new Claim(OpenIddictConstants.Claims.Email, "some@email").SetDestinations(OpenIddictConstants.Destinations.IdentityToken)
            };

        var claimsIdentity = new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Set requested scopes (this is not done automatically)
        claimsPrincipal.SetScopes(request.GetScopes());

        // Signing in with the OpenIddict authentiction scheme trigger OpenIddict to issue a code (which can be exchanged for an access token)
        return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}