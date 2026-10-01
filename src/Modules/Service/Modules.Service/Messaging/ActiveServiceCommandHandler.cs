namespace Modules.Service.Messaging;

internal sealed class ActivateServiceCommandHandler(
    ServiceDbContext context)
    : ICommandHandler<ActivateServiceCommand>
{
    public async Task<Result> Handle(
        ActivateServiceCommand command,
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
            ServiceStatusCode.Active);

        await context.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}
