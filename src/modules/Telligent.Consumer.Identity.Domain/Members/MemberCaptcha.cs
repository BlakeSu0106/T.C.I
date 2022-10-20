using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telligent.Core.Domain.Entities;

namespace Telligent.Consumer.Identity.Domain.Members;

/// <summary>
/// 驗證碼
/// </summary>
[Table("member_captcha")]
public class MemberCaptcha : Entity
{
    /// <summary>
    /// 驗證目標
    /// </summary>
    [Required]
    [StringLength(50)]
    [Column("key")]
    public string Key { get; set; }

    /// <summary>
    /// 驗證碼
    /// </summary>
    [StringLength(10)]
    [Column("value")]
    public string Value { get; set; }
}