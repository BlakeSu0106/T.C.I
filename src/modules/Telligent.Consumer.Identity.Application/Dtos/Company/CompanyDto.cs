using Telligent.Consumer.Identity.Application.Dtos.Corporation;
using Telligent.Consumer.Identity.Application.Dtos.Tenant;
using Telligent.Core.Application.DataTransferObjects;

namespace Telligent.Consumer.Identity.Application.Dtos.Company;

public class CompanyDto : EntityDto
{
    public TenantDto Tenant { get; set; }

    public CorporationDto Corporation { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}