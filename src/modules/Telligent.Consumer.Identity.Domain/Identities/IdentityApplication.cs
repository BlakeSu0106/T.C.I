using System.ComponentModel.DataAnnotations.Schema;
using OpenIddict.EntityFrameworkCore.Models;

namespace Telligent.Consumer.Identity.Domain.Identities;

[Table("identity_application")]
public class IdentityApplication : OpenIddictEntityFrameworkCoreApplication<Guid, IdentityAuthorization, IdentityToken>
{
    [Column("id")] public override Guid Id { get; set; }

    [Column("client_id")] public override string ClientId { get; set; }

    [Column("client_secret")] public override string ClientSecret { get; set; }

    [Column("concurrency_token")] public override string ConcurrencyToken { get; set; }

    [Column("consent_type")] public override string ConsentType { get; set; }

    [Column("display_name")] public override string DisplayName { get; set; }

    [Column("display_names")] public override string DisplayNames { get; set; }

    [Column("permissions")] public override string Permissions { get; set; }

    [Column("post_logout_redirect_uris")] public override string PostLogoutRedirectUris { get; set; }

    [Column("properties")] public override string Properties { get; set; }

    [Column("redirect_uris")] public override string RedirectUris { get; set; }

    [Column("requirements")] public override string Requirements { get; set; }

    [Column("type")] public override string Type { get; set; }
}