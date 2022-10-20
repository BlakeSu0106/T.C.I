using System.ComponentModel.DataAnnotations.Schema;
using OpenIddict.EntityFrameworkCore.Models;

namespace Telligent.Consumer.Identity.Domain.Identities;

[Table("identity_authorization")]
public class
    IdentityAuthorization : OpenIddictEntityFrameworkCoreAuthorization<Guid, IdentityApplication, IdentityToken>
{
    [Column("id")] public override Guid Id { get; set; }

    [Column("application_id")] public override IdentityApplication Application { get; set; }

    [Column("concurrency_token")] public override string ConcurrencyToken { get; set; }

    [Column("creation_date")] public override DateTime? CreationDate { get; set; }

    [Column("properties")] public override string Properties { get; set; }

    [Column("scopes")] public override string Scopes { get; set; }

    [Column("status")] public override string Status { get; set; }

    [Column("subject")] public override string Subject { get; set; }

    [Column("type")] public override string Type { get; set; }
}