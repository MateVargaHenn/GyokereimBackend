using Modules.Service.Domain;

namespace Modules.Service.Infrastructure.Persistence.Seed;

internal class OrderStatusSeeder(ServiceDbContext context) : EnumSeeder(context)
{
    public async Task SeedDataAsync()
    {
        foreach (OrderStatusCode enumItem in Enum.GetValues<OrderStatusCode>())
        {
            string name = enumItem switch
            {
                OrderStatusCode.Created => "Created",
                OrderStatusCode.Consultation => "Consultation",
                OrderStatusCode.InProgress => "In Progress",
                OrderStatusCode.Completed => "Completed",
                OrderStatusCode.Storno => "Storno",
                _ => "Unknown"
            };
            SeedEnum<OrderStatusCode, OrderStatus>(enumItem, name);
        }
        await context.SaveChangesAsync();
    }
}
