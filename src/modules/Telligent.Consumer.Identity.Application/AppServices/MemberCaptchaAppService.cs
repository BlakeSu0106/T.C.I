using Telligent.Core.Application.Services.Notifications;
using Telligent.Core.Domain.Notifications;
using Telligent.Core.Infrastructure.Captcha;
using Telligent.Core.Infrastructure.Extensions;
using Telligent.Core.Infrastructure.Generators;
using Telligent.Core.Infrastructure.Services;
using Telligent.Consumer.Identity.Application.Dtos.MemberCaptcha;
using Telligent.Consumer.Identity.Domain.Members;

namespace Telligent.Consumer.Identity.Application.AppServices;

public class MemberCaptchaAppService : IAppService
{
    private readonly MemberAppService _memberService;
    private readonly SmsPushService _smsService;
    private readonly UnitOfWork _uow;

    public MemberCaptchaAppService(
        UnitOfWork uow,
        SmsPushService smsService,
        MemberAppService memberService)
    {
        _uow = uow;
        _smsService = smsService;
        _memberService = memberService;
    }

    /// <summary>
    /// 發送驗證碼
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<PushResult> SendCaptchaAsync(CreateMemberCaptchaDto dto)
    {
        var captcha = new MemberCaptcha
        {
            Id = SequentialGuidGenerator.Instance.GetGuid(),
            Key = dto.Key,
            Value = CaptchaHelper.RandomString(6)
        };

        var list = await _uow.MemberCaptchaRepository.GetListAsync(ac => ac.Key.Equals(dto.Key));

        if (list.Any())
        {
            var last = list.Max(l => l.CreationTime);

            if (last.HasValue && DateTime.UtcNow.ToUtc8DateTime().Subtract(last.Value).Minutes < 1)
                throw new Exception("seconds not over 60");
        }

        await _uow.MemberCaptchaRepository.CreateAsync(captcha);

        await _uow.SaveChangeAsync();

        return await _smsService.PushAsync("80", captcha.Key, captcha.Value);
    }

    /// <summary>
    /// 檢查驗證碼
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<string> ValidateCaptchaAsync(ValidateMemberCaptchaDto dto)
    {
        if (string.IsNullOrEmpty(dto.Key))
            throw new Exception("mobile is null");

        var list = await _uow.MemberCaptchaRepository.GetListAsync(ac => ac.Key.Equals(dto.Key));

        var memberDto = await _memberService.GetAsync(g => g.Mobile.Equals(dto.Key));
        if (memberDto == null)
            throw new Exception("mobile is not exist");

        return list.Any() && list.OrderByDescending(l => l.CreationTime).FirstOrDefault()!.Value.Equals(dto.Value) ? memberDto.Id.ToString() : "false";
    }
}