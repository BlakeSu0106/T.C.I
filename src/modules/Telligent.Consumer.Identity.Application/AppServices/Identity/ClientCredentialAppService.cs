using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Telligent.Consumer.Identity.Application.Auth;
using Telligent.Consumer.Identity.Application.Extensions;
using Telligent.Core.Infrastructure.Services;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Telligent.Consumer.Identity.Application.AppServices.Identity;

public class ClientCredentialAppService : ControllerBase, IAppService
{
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictScopeManager _scopeManager;

    public ClientCredentialAppService(
        IOpenIddictApplicationManager applicationManager,
        IOpenIddictScopeManager scopeManager)
    {
        _applicationManager = applicationManager;
        _scopeManager = scopeManager;
    }

    /// <summary>
    /// client credential grant flow authentication
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<IActionResult> ExchangeAsync(OpenIddictRequest request)
    {
        var application = await _applicationManager.FindByClientIdAsync(request.ClientId ?? string.Empty);

        if (application == null)
            throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            Claims.Name, Claims.Role);

        var clientId = await _applicationManager.GetClientIdAsync(application);
        var name = await _applicationManager.GetDisplayNameAsync(application);

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(name))
            throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

        identity.AddClaim(Claims.Subject, clientId);
        identity.AddClaim(Claims.Name, name);

        identity.SetScopes(request.GetScopes());
        identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
        identity.SetDestinations(IdentityHelper.GetDestinations);

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}