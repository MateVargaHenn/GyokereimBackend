using Common.SharedKernel.Database;

namespace Modules.WebSetting.Domain;

public sealed class WebSetting : Entity
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
