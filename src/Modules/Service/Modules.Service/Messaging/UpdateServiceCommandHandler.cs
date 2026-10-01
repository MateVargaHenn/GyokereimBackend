namespace Modules.Service.Messaging;

internal sealed class UpdateServiceCommandHandler(
    ServiceDbContext context)
    : ICommandHandler<UpdateServiceCommand>
{
    public async Task<Result> Handle(
        UpdateServiceCommand command,
        CancellationToken cancellationToken)
    {
        Domain.Service? service =
            await context.Services
                .FirstOrDefaultAsync(
                    x => x.Id == command.ServiceId,
                    cancellationToken);

        if (service is null)
        {
            return Result.Failure(
                ErrorType.NotFound);
        }

        CreateServiceDto dto =
            command.ServiceDto;

        service.ChangeName(dto.Name);

        service.ChangeShortDescription(
            dto.ShortDescription);

        service.ChangeDescription(
            dto.Description);

        service.ChangeSlug(
            dto.Slug);

        service.ChangePrice(
            dto.Price);

        service.ChangeOrderNumber(dto.OrderNumber);

        service.ChangeStatus(
            dto.ServiceStatusCode);

        await context.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}
