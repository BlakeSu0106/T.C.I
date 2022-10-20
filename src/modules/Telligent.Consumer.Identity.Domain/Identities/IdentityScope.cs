using System.ComponentModel.DataAnnotations.Schema;
using OpenIddict.EntityFrameworkCore.Models;

namespace Telligent.Consumer.Identity.Domain.Identities;

[Table("identity_scope")]
public class IdentityScope : OpenIddictEntityFrameworkCoreScope<Guid>
{
    [Column("id")] public override Guid Id { get; set; }

    [Column("concurrency_token")] public override string ConcurrencyToken { get; set; }

    [Column("description")] public override string Description { get; set; }

    [Column("descriptions")] public override string Descriptions { get; set; }

    [Column("display_name")] public override string DisplayName { get; set; }

    [Column("display_names")] public override string DisplayNames { get; set; }

    [Column("name")] public override string Name { get; set; }

    [Column("properties")] public override string Properties { get; set; }

    [Column("resources")] public override string Resources { get; set; }
}