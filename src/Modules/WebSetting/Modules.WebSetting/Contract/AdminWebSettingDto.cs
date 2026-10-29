namespace Modules.WebSetting.Contract;

public sealed record AdminWebSettingDto(
    int Id,
    string Key,
    string Value,
    DateTime LastModified,
    string ModifiedBy);
