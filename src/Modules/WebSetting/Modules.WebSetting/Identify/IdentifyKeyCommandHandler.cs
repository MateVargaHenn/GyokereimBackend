using Common.SharedKernel.Common;
using Common.SharedKernel.Messaging;

namespace Modules.WebSetting.Identify;

internal sealed class IdentifyKeyCommandHandler(WebSettingDbContext dbContext)
    : ICommandHandler<IdentifyKeyCommand, WebSettingValueResponse>
{
    public async Task<Result<WebSettingValueResponse>> Handle(
        IdentifyKeyCommand command, 
        CancellationToken cancellationToken)
    {
        string? value = await dbContext.WebSettings
            .AsNoTracking()
            .Where(x => x.Key == command.Key)
            .Select(x => x.Value)
            .FirstOrDefaultAsync(cancellationToken);

        if (value is null)
        {
            return Result.Failure<WebSettingValueResponse>(Error.NullValue);
        }

        return Result.Success<WebSettingValueResponse>(new WebSettingValueResponse(value));
    }
}
