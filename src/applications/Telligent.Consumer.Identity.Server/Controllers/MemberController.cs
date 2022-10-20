using Microsoft.AspNetCore.Mvc;
using Telligent.Consumer.Identity.Application.AppServices;
using Telligent.Consumer.Identity.Application.Dtos.Member;

namespace Telligent.Consumer.Identity.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MemberController : ControllerBase
{
    private readonly MemberAppService _memberAppService;

    public MemberController(MemberAppService memberAppService)
    {
        _memberAppService = memberAppService;
    }

    [HttpPost("registration")]
    public async Task<IActionResult> RegisterAsync(CreateMemberDto dto)
    {
        return Ok(await _memberAppService.CreateAsync(dto));
    }
}