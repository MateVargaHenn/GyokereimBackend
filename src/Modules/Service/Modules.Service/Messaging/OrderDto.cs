namespace Modules.Service.Messaging;

public record OrderDto(string CustomerName, string Email, string PhoneNumber, string? Message, string ServiceKey, bool AcceptTerms);
