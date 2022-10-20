using System.Security.Claims;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Telligent.Consumer.Identity.Application.Auth
{
    public static class IdentityHelper
    {
        public static IEnumerable<string> GetDestinations(Claim claim)
        {
            switch (claim.Type)
            {
                case OpenIddictConstants.Claims.Name:
                case OpenIddictConstants.Claims.Subject:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if (claim.Subject != null && claim.Subject.HasScope(OpenIddictConstants.Permissions.Scopes.Profile))
                        yield return OpenIddictConstants.Destinations.IdentityToken;

                    yield break;
                case Claims.Email:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if (claim.Subject != null && claim.Subject.HasScope(OpenIddictConstants.Permissions.Scopes.Email))
                        yield return OpenIddictConstants.Destinations.IdentityToken;

                    yield break;

                case Claims.Role:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if (claim.Subject != null && claim.Subject.HasScope(OpenIddictConstants.Permissions.Scopes.Roles))
                        yield return OpenIddictConstants.Destinations.IdentityToken;

                    yield break;

                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                case "AspNet.Identity.SecurityStamp": yield break;

                default:
                    yield return OpenIddictConstants.Destinations.AccessToken;
                    yield break;
            }
        }
    }
}
