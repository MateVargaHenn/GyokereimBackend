using Modules.WebSetting.Contract;

namespace Modules.WebSetting.Application.GetWebSettingsForAdmin;

public sealed record GetWebSettingsForAdminQuery
    : IRequest<IReadOnlyCollection<AdminWebSettingDto>>;
