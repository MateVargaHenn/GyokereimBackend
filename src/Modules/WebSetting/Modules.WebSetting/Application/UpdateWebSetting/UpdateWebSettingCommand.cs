using MediatR;

namespace Modules.WebSetting.Application.UpdateWebSetting;

public sealed record UpdateWebSettingCommand(
    string Key,
    string Value)
    : IRequest<bool>;
