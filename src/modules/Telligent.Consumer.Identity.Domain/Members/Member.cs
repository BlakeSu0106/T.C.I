using System.ComponentModel.DataAnnotations.Schema;
using Telligent.Core.Domain.Entities;

namespace Telligent.Consumer.Identity.Domain.Members;

[Table("member")]
public class Member : Entity
{
    [Column("corporation_id")]
    public Guid CorporationId { get; set; }

    [Column("company_id")]
    public Guid CompanyId { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("user_id")]
    public string UserId { get; set; }

    [Column("mobile")]
    public string Mobile { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("password")]
    public string Password { get; set; }

    [Column("registration_time")]
    public DateTime RegistrationTime { get; set; }
}