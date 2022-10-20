using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;

namespace Telligent.Consumer.Identity.Application.Localization;

public class LocalizeRequiredAttribute : RequiredAttribute
{
    private readonly IStringLocalizer<LocalizeResource> _localizer;

    public LocalizeRequiredAttribute(IStringLocalizer<LocalizeResource> localizer)
    {
        _localizer = localizer;
    }

    public override string FormatErrorMessage(string name)
    {
        return _localizer.GetString(name);
    }
}