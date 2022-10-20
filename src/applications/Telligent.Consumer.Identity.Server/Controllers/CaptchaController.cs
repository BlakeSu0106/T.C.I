using Microsoft.AspNetCore.Mvc;
using Telligent.Consumer.Identity.Application.AppServices;
using Telligent.Consumer.Identity.Application.Dtos.MemberCaptcha;

namespace Telligent.Consumer.Identity.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CaptchaController : ControllerBase
{
    private readonly MemberCaptchaAppService _memberCaptchaService;

    public CaptchaController(MemberCaptchaAppService memberCaptchaService)
    {
        _memberCaptchaService = memberCaptchaService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendCaptcha(CreateMemberCaptchaDto dto)
    {
        try
        {
            return Ok(await _memberCaptchaService.SendCaptchaAsync(dto));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidateCaptcha(ValidateMemberCaptchaDto dto)
    {
        try
        {
            return Ok(await _memberCaptchaService.ValidateCaptchaAsync(dto));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
}