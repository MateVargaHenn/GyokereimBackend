using Common.SharedKernel.Messaging;

namespace Modules.WebSetting.Identify;

public class IdentifyKeyCommand(string Key) : ICommand<WebSettingValueResponse>
{
    public string Key { get; private set; } = Key;
}
