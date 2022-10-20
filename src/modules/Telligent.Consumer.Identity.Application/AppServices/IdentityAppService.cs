using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Telligent.Consumer.Identity.Application.AppServices.Identity;
using Telligent.Core.Infrastructure.Services;

namespace Telligent.Consumer.Identity.Application.AppServices;

public class IdentityAppService : IAppService
{
    private readonly ClientCredentialAppService _clientCredentialAppService;
    private readonly ResourceOwnerPasswordCredentialAppService _ropcAppService;
    private readonly AuthorizationCodeAppService _authorizationCodeAppService;

    public IdentityAppService(
        ClientCredentialAppService clientCredentialAppService, 
        ResourceOwnerPasswordCredentialAppService ropcAppService,
        AuthorizationCodeAppService authorizationCodeAppService)
    {
        _clientCredentialAppService = clientCredentialAppService;
        _ropcAppService = ropcAppService;
        _authorizationCodeAppService = authorizationCodeAppService;
    }

    public async Task<IActionResult> ExchangeAsync(OpenIddictRequest request)
    {
        if (request == null)
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (request.IsClientCredentialsGrantType())
            return await _clientCredentialAppService.ExchangeAsync(request);

        if (request.IsPasswordGrantType())
            return await _ropcAppService.ExchangeAsync(request);

        if (!request.IsAuthorizationCodeGrantType() && !request.IsRefreshTokenGrantType())
            return await _authorizationCodeAppService.ExchangeAsync();

        throw new InvalidOperationException("The specified grant type is not supported.");
    }

    public async Task<IActionResult> AuthorizeAsync(OpenIddictRequest request)
    {
        return await _authorizationCodeAppService.AuthorizeAsync(request);
    }
}