using System.ComponentModel.DataAnnotations.Schema;
using Telligent.Core.Domain.Entities;

namespace Telligent.Consumer.Identity.Domain.Organizations;

[Table("corporation")]
public class Corporation : Entity
{
    /// <summary>
    /// 集團名稱
    /// </summary>
    [Column("name")]
    public string Name { get; set; }


    /// <summary>
    /// 集團簡寫
    /// </summary>
    [Column("short_name")]
    public string ShortName { get; set; }
}