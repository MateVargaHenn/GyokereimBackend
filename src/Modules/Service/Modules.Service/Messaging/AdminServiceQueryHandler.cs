namespace Modules.Service.Messaging;

internal sealed class AdminServiceQueryHandler(
    ServiceDbContext context)
    : IQueryHandler<AdminServiceQuery, AdminServiceDto>
{
    public async Task<Result<AdminServiceDto>> Handle(
        AdminServiceQuery request,
        CancellationToken cancellationToken)
    {
        AdminServiceDto? service =
            await context.Services
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
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
                .FirstOrDefaultAsync(cancellationToken);

        if (service is null)
        {
            return Result
                .Failure<AdminServiceDto>(Error.NotFound);
        }

        return Result<AdminServiceDto>
            .Success(service);
    }
}
