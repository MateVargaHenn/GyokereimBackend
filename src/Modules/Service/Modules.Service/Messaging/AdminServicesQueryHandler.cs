



namespace Modules.Service.Messaging;

internal sealed class AdminServicesQueryHandler(
    ServiceDbContext context)
    : IQueryHandler<
        AdminServicesQuery,
        List<AdminServiceDto>>
{
    public async Task<Result<List<AdminServiceDto>>> Handle(
        AdminServicesQuery request,
        CancellationToken cancellationToken)
    {
        List<AdminServiceDto> services =
            await context.Services
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x =>
                    new AdminServiceDto(
                        x.Id,
                        x.Name,
                        x.Slug,
                        x.ShortDescription,
                        x.Description,
                        x.Price,
                        x.OrderNumber,
                        x.ServiceStatusCode))
                .ToListAsync(cancellationToken);

        return Result<List<AdminServiceDto>>
            .Success(services);
    }
}
