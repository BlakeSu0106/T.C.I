using System.ComponentModel.DataAnnotations.Schema;
using OpenIddict.EntityFrameworkCore.Models;

namespace Telligent.Consumer.Identity.Domain.Identities;

[Table("identity_token")]
public class IdentityToken : OpenIddictEntityFrameworkCoreToken<Guid, IdentityApplication, IdentityAuthorization>
{
    [Column("id")] public override Guid Id { get; set; }

    [Column("application_id")] public override IdentityApplication Application { get; set; }

    [Column("authorization_id")] public override IdentityAuthorization Authorization { get; set; }

    [Column("concurrency_token")] public override string ConcurrencyToken { get; set; }

    [Column("creation_date")] public override DateTime? CreationDate { get; set; }

    [Column("expiration_date")] public override DateTime? ExpirationDate { get; set; }

    [Column("payload")] public override string Payload { get; set; }

    [Column("properties")] public override string Properties { get; set; }

    [Column("redemption_date")] public override DateTime? RedemptionDate { get; set; }

    [Column("reference_id")] public override string ReferenceId { get; set; }

    [Column("status")] public override string Status { get; set; }

    [Column("subject")] public override string Subject { get; set; }

    [Column("type")] public override string Type { get; set; }
}