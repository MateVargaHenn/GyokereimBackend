namespace Modules.Service.Messaging;

internal sealed record AdminServiceQuery(int Id)
    : IQuery<AdminServiceDto>;
