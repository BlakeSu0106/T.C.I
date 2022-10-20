using System.ComponentModel.DataAnnotations.Schema;
using Telligent.Core.Domain.Entities;

namespace Telligent.Consumer.Identity.Domain.Organizations;

[Table("company")]
public class Company : Entity
{
    /// <summary>
    /// 集團識別碼
    /// </summary>
    [Column("corporation_id")]
    public Guid CorporationId { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    [Column("name")]
    public string Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [Column("description")]
    public string Description { get; set; }
}