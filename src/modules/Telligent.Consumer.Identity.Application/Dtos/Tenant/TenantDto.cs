using Telligent.Consumer.Identity.Domain.Shared;
using Telligent.Core.Application.DataTransferObjects;

namespace Telligent.Consumer.Identity.Application.Dtos.Tenant;

public class TenantDto : EntityDto
{
    public string Name { get; set; }

    public EnterpriseType EnterpriseType { get; set; }
}