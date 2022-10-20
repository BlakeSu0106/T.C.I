using Telligent.Core.Application.DataTransferObjects;

namespace Telligent.Consumer.Identity.Application.Dtos.Corporation;

public class CorporationDto : EntityDto
{
    public string Name { get; set; }

    public string ShortName { get; set; }
}