using MediatR;
using Microsoft.EntityFrameworkCore;
using Modules.WebSetting.Contract;
using Modules.WebSetting.Infrastructure.Persistence;

namespace Modules.WebSetting.Application.GetWebSettingsForAdmin;

internal sealed class GetWebSettingsForAdminQueryHandler(
    WebSettingDbContext dbContext)
    : IRequestHandler<
        GetWebSettingsForAdminQuery,
        IReadOnlyCollection<AdminWebSettingDto>>
{
    public async Task<IReadOnlyCollection<AdminWebSettingDto>> Handle(
        GetWebSettingsForAdminQuery request,
        CancellationToken cancellationToken) => await dbContext
            .WebSettings
            .AsNoTracking()
            .OrderBy(x => x.Key)
            .Select(x => new AdminWebSettingDto(
                x.Id,
                x.Key,
                x.Value,
                x.LastModified,
                x.ModifiedBy))
            .ToListAsync(cancellationToken);
}
