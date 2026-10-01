using Common.SharedKernel.Messaging;
using Modules.Service.Contracts;

namespace Modules.Service.Messaging;

internal sealed class ServiceQuery(string Slug) : IQuery<ServiceDto>
{
    public string Slug { get; private set; } = Slug;
}
