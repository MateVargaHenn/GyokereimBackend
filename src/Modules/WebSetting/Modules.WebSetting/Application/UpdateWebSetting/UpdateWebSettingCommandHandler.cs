using MediatR;
using Microsoft.EntityFrameworkCore;
using Modules.WebSetting.Infrastructure.Persistence;

namespace Modules.WebSetting.Application.UpdateWebSetting;

internal sealed class UpdateWebSettingCommandHandler(
    WebSettingDbContext dbContext)
    : IRequestHandler<UpdateWebSettingCommand, bool>
{
    public async Task<bool> Handle(
        UpdateWebSettingCommand request,
        CancellationToken cancellationToken)
    {
        Domain.WebSetting? webSetting =
            await dbContext
                .WebSettings
                .SingleOrDefaultAsync(
                    x => x.Key == request.Key,
                    cancellationToken);

        if (webSetting is null)
        {
            return false;
        }

        webSetting.Value = request.Value;

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
