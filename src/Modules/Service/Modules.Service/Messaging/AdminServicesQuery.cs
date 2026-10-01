using Common.SharedKernel.Messaging;
using Modules.Service.Contracts;

namespace Modules.Service.Messaging;

internal sealed record AdminServicesQuery
    : IQuery<List<AdminServiceDto>>;
