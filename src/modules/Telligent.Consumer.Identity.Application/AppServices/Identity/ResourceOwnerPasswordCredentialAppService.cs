using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Telligent.Consumer.Identity.Application.Auth;
using Telligent.Core.Infrastructure.Services;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Telligent.Consumer.Identity.Application.AppServices.Identity;

public class ResourceOwnerPasswordCredentialAppService : ControllerBase, IAppService
{
    private readonly MemberAppService _memberAppService;

    public ResourceOwnerPasswordCredentialAppService(
        MemberAppService memberAppService)
    {
        _memberAppService = memberAppService;
    }

    /// <summary>
    /// resource owner password credential flow authentication
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<IActionResult> ExchangeAsync(OpenIddictRequest request)
    {
        if (string.IsNullOrEmpty(request["companyId"].ToString()) || string.IsNullOrEmpty(request.Username) ||
            string.IsNullOrEmpty(request.Password))
        {
            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                    "The username/password couple is invalid."
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var member =
            await _memberAppService.GetAsync(request["companyId"].ToString(), request.Username, request.Password);

        if (member == null)
        {
            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                    "The username/password couple is invalid."
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)
            .AddClaim(Claims.Subject, member.Id.ToString()) // id
            .AddClaim(Claims.Email, member.Email)
            .AddClaim(Claims.Name, member.Name);

        // Set the list of scopes granted to the client application.
        identity.SetScopes(new[]
        {
            Scopes.OpenId,
            Scopes.Email,
            Scopes.Profile,
            Scopes.Roles
        }.Intersect(request.GetScopes()));

        identity.SetDestinations(IdentityHelper.GetDestinations);

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}