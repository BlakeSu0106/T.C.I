using System.ComponentModel.DataAnnotations.Schema;
using Telligent.Consumer.Identity.Domain.Shared;
using Telligent.Core.Domain.Entities;

namespace Telligent.Consumer.Identity.Domain.Organizations;

/// <summary>
/// 租戶
/// </summary>
[Table("tenant")]
public class Tenant : Entity
{
    /// <summary>
    /// 名稱
    /// </summary>
    [Column("name")]
    public string Name { get; set; }

    /// <summary>
    /// 企業類型
    /// </summary>
    [Column("enterprise_type")]
    public EnterpriseType EnterpriseType { get; set; }
}