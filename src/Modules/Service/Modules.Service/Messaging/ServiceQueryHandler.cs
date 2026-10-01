using Common.SharedKernel.Messaging;
using Modules.Service.Contracts;

namespace Modules.Service.Messaging;

internal sealed class ServiceQueryHandler(ServiceDbContext context) : IQueryHandler<ServiceQuery, ServiceDto>
{
    private readonly ServiceDbContext _context = context;
    public async Task<Result<ServiceDto>> Handle(ServiceQuery request, CancellationToken cancellationToken)
    {
        Domain.Service? service = await _context.Services.SingleOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);
        if (service is null)
        {
            return Result.Failure<ServiceDto>(Error.NotFound);
        }

        return Result<ServiceDto>.Success(new ServiceDto(service.Name, service.Slug, service.ShortDescription, service.Description, service.Price));
    }
}
