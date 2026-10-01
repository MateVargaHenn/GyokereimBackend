namespace Modules.Service.Messaging;

internal class ServicesQueryHandler(ServiceDbContext context) : IQueryHandler<ServicesQuery, List<ServiceDto>>
{
    private readonly ServiceDbContext _context = context;
    public async Task<Result<List<ServiceDto>>> Handle(ServicesQuery request, CancellationToken cancellationToken)
    {
        List<Domain.Service> services = await _context.Services.Where(x => x.ServiceStatusCode == ServiceStatusCode.Active).OrderBy(x =>x.OrderNumber).ToListAsync(cancellationToken);
        return Result<List<ServiceDto>>.Success(services.Select(s => new ServiceDto(s.Name, s.Slug, s.ShortDescription, s.Description, s.Price)).ToList());
    }
}
