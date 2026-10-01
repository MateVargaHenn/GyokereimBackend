namespace Common.Infrastructure.Messaging;

public sealed class MassTransitIntegrationEventPublisher
    : IIntegrationEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitIntegrationEventPublisher(
        IPublishEndpoint publishEndpoint) => _publishEndpoint = publishEndpoint;

    public async Task PublishAsync<T>(
        T integrationEvent,
        CancellationToken cancellationToken = default)
        where T : class
    {
        await _publishEndpoint.Publish(
            integrationEvent,
            cancellationToken);

        Type type = integrationEvent.GetType();
    }
}
