using Modules.Chat.Application;
using Modules.Chat.Models;

namespace Modules.Chat.Ask;

internal sealed class AskChatCommandHandler(
    IIntentClassifier intentClassifier,
    ChatDbContext dbContext,
    IServiceInformationProvider serviceInformationProvider,
    ILogger<AskChatCommandHandler> logger)
    : ICommandHandler<AskChatCommand, ChatResponseDto>
{
    private readonly ILogger<AskChatCommandHandler> _logger = logger;
    public async Task<Result<ChatResponseDto>> Handle(
    AskChatCommand command,
    CancellationToken cancellationToken)
    {
        string message = command.ChatRequest.Message;

        (string intentKey, string? greetingIntent) =
            ResolveIntents(message);

        int? ordinalIndex = FindOrdinalIndex(message);
        if (ordinalIndex.HasValue &&
    command.ChatRequest.ReferencedServiceIds is { Count: > 1 } &&
    intentKey is not "service_price"
        and not "service_order"
        and not "service_details")
        {
            intentKey = "service_select";
        }

        if (command.ChatRequest.ReferencedServiceIds is { Count: 1 } &&
    IsServiceDetailsFollowUp(message))
        {
            intentKey = "service_details";
        }

        if (intentKey == "service_price")
        {
            IReadOnlyCollection<ServiceInformation> services =
                await serviceInformationProvider
                    .GetServicesAsync(cancellationToken);

            IReadOnlyCollection<ServiceInformation> mentionedServices =
                ResolveServices(
                    message,
                    services,
                    command.ChatRequest.ReferencedServiceIds);

            string answer = string.Empty;
            if (mentionedServices.Count == 0)
            {
                answer = ApplyGreeting(
                    "Melyik szolgáltatás árára gondol?",
                    greetingIntent);

                var chatResponseDto = new ChatResponseDto(
                    message,
                    new ChatAnswerDto(answer),
                    intentKey,
                    []);

                return Result.Success(chatResponseDto);
            }

            answer = string.Join(
                Environment.NewLine,
                mentionedServices.Select(service =>
                    $"{service.Name}: {service.Price:N0} Ft"));

            answer = ApplyGreeting(
                answer,
                greetingIntent);

            var response = new ChatResponseDto(
                message,
                new ChatAnswerDto(answer),
                intentKey,
                mentionedServices.Select(s => s.Id).ToList());

            return Result.Success(response);
        }

        if (intentKey == "service_details")
        {
            IReadOnlyCollection<ServiceInformation> services =
                await serviceInformationProvider
                    .GetServicesAsync(cancellationToken);

            IReadOnlyCollection<ServiceInformation> mentionedServices =
                ResolveServices(
                    message,
                    services,
                    command.ChatRequest.ReferencedServiceIds);

            if (mentionedServices.Count == 0)
            {
                var response = new ChatResponseDto(
                    message,
                    new ChatAnswerDto(
                        "Melyik szolgáltatásról szeretne többet megtudni?"),
                    intentKey,
                    command.ChatRequest.ReferencedServiceIds ?? []);

                return Result.Success(response);
            }

            if (mentionedServices.Count > 1)
            {
                var response = new ChatResponseDto(
                    message,
                    new ChatAnswerDto(
                        "Melyik szolgáltatás részleteire kíváncsi?"),
                    intentKey,
                    mentionedServices
                        .Select(x => x.Id)
                        .ToArray());

                return Result.Success(response);
            }

            ServiceInformation service =
                mentionedServices.First();

            string answer =
                $"{service.Name}: {service.Description}";

            answer = ApplyGreeting(
                answer,
                greetingIntent);

            var chatResponse = new ChatResponseDto(
                message,
                new ChatAnswerDto(answer),
                intentKey,
                [service.Id]);

            return Result.Success(chatResponse);
        }

        if (intentKey == "service_select")
        {
            IReadOnlyCollection<ServiceInformation> services =
                await serviceInformationProvider
                    .GetServicesAsync(cancellationToken);

            IReadOnlyCollection<ServiceInformation> mentionedServices =
                ResolveServices(
                    message,
                    services,
                    command.ChatRequest.ReferencedServiceIds);

            if (mentionedServices.Count == 0)
            {
                var response = new ChatResponseDto(
                    message,
                    new ChatAnswerDto(
                        "Melyik szolgáltatást választaná?"),
                    intentKey,
                    command.ChatRequest.ReferencedServiceIds ?? []);

                return Result.Success(response);
            }

            if (mentionedServices.Count > 1)
            {
                var response = new ChatResponseDto(
                    message,
                    new ChatAnswerDto(
                        "Pontosan melyik szolgáltatást választaná?"),
                    intentKey,
                    mentionedServices
                        .Select(x => x.Id)
                        .ToArray());

                return Result.Success(response);
            }

            ServiceInformation service =
                mentionedServices.First();

            string answer =
                $"Rendben, a {service.Name} szolgáltatást választotta. " +
                "Szeretné megrendelni, vagy szeretne még többet megtudni róla?";

            answer = ApplyGreeting(
                answer,
                greetingIntent);

            var chatResponse = new ChatResponseDto(
                message,
                new ChatAnswerDto(answer),
                intentKey,
                [service.Id]);

            return Result.Success(chatResponse);
        }

        if (intentKey == "service_order")
        {
            IReadOnlyCollection<ServiceInformation> services =
                await serviceInformationProvider
                    .GetServicesAsync(cancellationToken);

            IReadOnlyCollection<ServiceInformation> mentionedServices =
            FindMentionedServices(
                message,
                services);

            if (mentionedServices.Count == 0
                && command.ChatRequest.ReferencedServiceIds is not null)
            {
                mentionedServices = services
                    .Where(service =>
                        command.ChatRequest.ReferencedServiceIds.Contains(
                            service.Id))
                    .ToList();
            }

            if (mentionedServices.Count == 0)
            {
                var response = new ChatResponseDto(
                    message,
                    new ChatAnswerDto(
                        "Melyik szolgáltatást szeretné megrendelni?"),
                    intentKey,
                    []);

                return Result.Success(response);
            }

            if (mentionedServices.Count > 1)
            {
                var response = new ChatResponseDto(
                    message,
                    new ChatAnswerDto(
                        "Melyik szolgáltatást szeretné megrendelni?"),
                    intentKey,
                    mentionedServices
                        .Select(x => x.Id)
                        .ToArray());

                return Result.Success(response);
            }

            ServiceInformation service =
                mentionedServices.First();

            var chatAnswer = new ChatAnswerDto(
                $"A {service.Name} szolgáltatást itt tudja megrendelni.",
                $"/megrendeles/{service.Key}");

            var chatResponse = new ChatResponseDto(
                message,
                chatAnswer,
                intentKey,
                [service.Id]);

            return Result.Success(chatResponse);
        }

        string? defaultAnswer = await dbContext.Answers
    .AsNoTracking()
    .Where(x => x.Intent.Key == intentKey)
    .Select(x => x.Text)
    .FirstOrDefaultAsync(cancellationToken);

        if (defaultAnswer is null)
        {

            _logger.LogWarning(
                "No chat answer found for predicted intent {Intent}. Message: {Message}",
                intentKey,
                message);

            var fallbackResponse = new ChatResponseDto(
                message,
                new ChatAnswerDto(
                    "Ezt most nem tudom biztosan értelmezni. Meg tudná fogalmazni másképp?"),
                "fallback",
                command.ChatRequest.ReferencedServiceIds ?? []);

            return Result.Success(fallbackResponse);
        }

        IReadOnlyCollection<ServiceInformation> availableServices =
            await serviceInformationProvider
                .GetServicesAsync(cancellationToken);

        IReadOnlyCollection<ServiceInformation> referencedServices =
            FindMentionedServices(
                defaultAnswer,
                availableServices);

        defaultAnswer = ApplyGreeting(
            defaultAnswer,
            greetingIntent);

        var defaultResponse = new ChatResponseDto(
            message,
            new ChatAnswerDto(defaultAnswer),
            intentKey,
            referencedServices
                .Select(x => x.Id)
                .ToArray());

        return Result.Success(defaultResponse);
    }

    private (string IntentKey, string? GreetingIntent) ResolveIntents(
    string message)
    {
        string[] parts = message.Split(
            ['!', '?', '.'],
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries);

        string? greetingIntent = null;
        string? mainIntent = null;

        foreach (string part in parts)
        {
            IntentPrediction prediction =
                intentClassifier.Predict(part);

            if (IsGreetingIntent(prediction.Intent))
            {
                greetingIntent ??= prediction.Intent;
                continue;
            }

            if (prediction.Confidence < 0.60f)
            {
                mainIntent ??= "fallback";
                continue;
            }

            mainIntent ??= prediction.Intent;
        }

        // Például csak ennyit írt:
        // "Szia!"
        if (mainIntent is null &&
            greetingIntent is not null)
        {
            return (greetingIntent, null);
        }

        // Biztonsági fallback
        if (mainIntent is null)
        {
            IntentPrediction prediction =
                intentClassifier.Predict(message);

            mainIntent = prediction.Intent;
        }

        return (
            mainIntent,
            greetingIntent);
    }
    private static bool IsGreetingIntent(
    string intent) => intent is
            "greeting_informal" or
            "greeting_formal";

    private static string ApplyGreeting(
    string answer,
    string? greetingIntent) => greetingIntent switch
    {
        "greeting_informal" =>
            $"Szia! {answer}",

        "greeting_formal" =>
            $"Üdvözlöm! {answer}",

        _ => answer
    };

    private static IReadOnlyCollection<ServiceInformation> FindMentionedServices(
        string input,
        IReadOnlyCollection<ServiceInformation> services) => services
            .Select(service => new
            {
                Service = service,
                Index = input.IndexOf(
                    service.Name,
                    StringComparison.CurrentCultureIgnoreCase)
            })
            .Where(x => x.Index >= 0)
            .OrderBy(x => x.Index)
            .Select(x => x.Service)
            .ToList();

    private static int? FindOrdinalIndex(string input)
    {
        string normalized = input.ToLowerInvariant();

        if (normalized.Contains("első")
            || normalized.Contains("1"))
        {
            return 0;
        }

        if (normalized.Contains("második")
            || normalized.Contains("2"))
        {
            return 1;
        }

        if (normalized.Contains("harmadik")
            || normalized.Contains("3"))
        {
            return 2;
        }

        return null;
    }

    private static IReadOnlyCollection<ServiceInformation> ResolveServices(
    string message,
    IReadOnlyCollection<ServiceInformation> services,
    IReadOnlyCollection<int>? referencedServiceIds)
    {
        IReadOnlyCollection<ServiceInformation> mentionedServices =
            FindMentionedServices(
                message,
                services);

        // Konkrétan megnevezte a szolgáltatást.
        if (mentionedServices.Count > 0)
        {
            return mentionedServices;
        }

        if (referencedServiceIds is null ||
            referencedServiceIds.Count == 0)
        {
            return [];
        }

        // Pl. "az elsőt választom"
        int? ordinalIndex =
            FindOrdinalIndex(message);

        if (ordinalIndex.HasValue)
        {
            int[] ids =
                referencedServiceIds.ToArray();

            if (ordinalIndex.Value >= ids.Length)
            {
                return [];
            }

            int selectedServiceId =
                ids[ordinalIndex.Value];

            ServiceInformation? selectedService =
                services.FirstOrDefault(
                    x => x.Id == selectedServiceId);

            return selectedService is null
                ? []
                : [selectedService];
        }

        // Nincs név és nincs sorszám:
        // pl. "mennyibe kerülnek?"
        return services
            .Where(service =>
                referencedServiceIds.Contains(service.Id))
            .OrderBy(service =>
                referencedServiceIds
                    .ToList()
                    .IndexOf(service.Id))
            .ToList();
    }

    private static bool IsServiceDetailsFollowUp(
    string message) => message.Contains(
                "többet megtudni",
                StringComparison.CurrentCultureIgnoreCase) ||

            message.Contains(
                "többet tudni",
                StringComparison.CurrentCultureIgnoreCase) ||

            message.Contains(
                "mesélj róla",
                StringComparison.CurrentCultureIgnoreCase) ||

            message.Contains(
                "meséljen róla",
                StringComparison.CurrentCultureIgnoreCase) ||

            message.Contains(
                "részletek",
                StringComparison.CurrentCultureIgnoreCase) ||

            message.Contains(
                "mit tartalmaz",
                StringComparison.CurrentCultureIgnoreCase) ||

            message.Contains(
                "miről szól",
                StringComparison.CurrentCultureIgnoreCase) ||

            message.Contains(
                "mi van benne",
                StringComparison.CurrentCultureIgnoreCase);
}
