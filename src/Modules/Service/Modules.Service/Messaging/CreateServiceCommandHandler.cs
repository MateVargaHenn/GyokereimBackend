namespace Modules.Service.Messaging;

internal sealed class CreateServiceCommandHandler(
    ServiceDbContext context)
    : ICommandHandler<CreateServiceCommand>
{
    public async Task<Result> Handle(
        CreateServiceCommand command,
        CancellationToken cancellationToken)
    {
        CreateServiceDto dto = command.ServiceDto;

        var service = Domain.Service.Create(
            dto.Name,
            dto.ShortDescription,
            dto.Description,
            dto.Slug,
            dto.Price,
            dto.OrderNumber,
            dto.ServiceStatusCode);

        context.Services.Add(service);

        await context.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}
