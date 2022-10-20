using Telligent.Core.Application.DataTransferObjects;

namespace Telligent.Consumer.Identity.Application.Dtos.Member;

public class CreateMemberDto : EntityDto
{
    public Guid CompanyId { get; set; }

    public string Name { get; set; }

    public string UserId { get; set; }

    public string Mobile { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
}