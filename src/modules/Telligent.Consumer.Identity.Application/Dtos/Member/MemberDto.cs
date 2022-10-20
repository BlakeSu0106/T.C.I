using Telligent.Core.Application.DataTransferObjects;

namespace Telligent.Consumer.Identity.Application.Dtos.Member;

public class MemberDto : EntityDto
{
    public Guid CompanyId { get; set; }

    public string Name { get; set; }

    public string UserId { get; set; }

    public string Mobile { get; set; }

    public string Email { get; set; }
}