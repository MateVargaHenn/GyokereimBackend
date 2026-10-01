namespace Modules.Service.Messaging;

internal sealed class DeactivateServiceCommandHandler(
    ServiceDbContext context)
    : ICommandHandler<DeactivateServiceCommand>
{
    public async Task<Result> Handle(
        DeactivateServiceCommand command,
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

        service.ChangeStatus(
            ServiceStatusCode.Inactive);

        await context.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}
