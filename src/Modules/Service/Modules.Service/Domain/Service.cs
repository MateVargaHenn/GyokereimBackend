using Modules.Service.Exceptions;

namespace Modules.Service.Domain;

internal sealed class Service : Entity
{
    internal string Name { get; private set; } = null!;
    internal string ShortDescription { get; private set; } = null!;
    internal string Description { get; private set; } = null!;
    internal string Slug { get; private set; } = null!;
    internal decimal Price { get; private set; } = 0.0m;
    internal int OrderNumber { get; private set; } = 0;
    internal ServiceStatusCode ServiceStatusCode { get; private set; } = ServiceStatusCode.Inactive;
    private readonly List<Order> _orders = [];

    internal IReadOnlyCollection<Order> Orders =>
        _orders;

    public static Service Create(
        string name,
        string shortDescription,
        string description,
        string slug,
        decimal price,
        int orderNumber,
        ServiceStatusCode serviceStatusCode)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidServiceNameException();
        }
        if (string.IsNullOrWhiteSpace(shortDescription))
        {
            throw new InvalidServiceShortDescriptionException();
        }
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidServiceDescriptionException();
        }
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new InvalidServiceSlugException();
        }
        if (price <= 0)
        {
            throw new InvalidServicePriceException();
        }
        if (!System.Enum.IsDefined(typeof(ServiceStatusCode), serviceStatusCode))
        {
            throw new InvalidServiceStatusCodeException();
        }

        return new Service
        {
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            Slug = slug,
            Price = price,
            OrderNumber = orderNumber,
            ServiceStatusCode = serviceStatusCode
        };
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidServiceNameException();
        }

        Name = name;
    }

    public void ChangeShortDescription(
        string shortDescription)
    {
        if (string.IsNullOrWhiteSpace(shortDescription))
        {
            throw new InvalidServiceShortDescriptionException();
        }

        ShortDescription = shortDescription;
    }

    public void ChangeDescription(
        string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidServiceDescriptionException();
        }

        Description = description;
    }

    public void ChangeSlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new InvalidServiceSlugException();
        }

        Slug = slug;
    }

    public void ChangePrice(decimal price)
    {
        if (price <= 0)
        {
            throw new InvalidServicePriceException();
        }

        Price = price;
    }

    public void ChangeStatus(
        ServiceStatusCode serviceStatusCode)
    {
        if (!System.Enum.IsDefined(serviceStatusCode))
        {
            throw new InvalidServiceStatusCodeException();
        }

        ServiceStatusCode = serviceStatusCode;
    }

    internal void ChangeOrderNumber(int orderNumber) => OrderNumber = orderNumber;
}
