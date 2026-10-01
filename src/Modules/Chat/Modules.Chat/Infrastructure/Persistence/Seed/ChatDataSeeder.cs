using MassTransit;

namespace Modules.Chat.Infrastructure.Persistence.Seed;

internal class ChatDataSeeder
{
    internal async Task SeedAsync(
        ChatDbContext context,
        CancellationToken cancellationToken = default)
    {
        Intent greetingInformal =
    await GetOrCreateIntentAsync(
        context,
        "greeting_informal",
        "Tegező Köszönés",
        cancellationToken);

        greetingInformal.AddUtterance("Szia");
        greetingInformal.AddUtterance("Helló");
        greetingInformal.AddUtterance("Üdv");
        greetingInformal.AddUtterance("Csá");
        greetingInformal.AddUtterance("Csáó");
        greetingInformal.AddUtterance("Hahó");
        greetingInformal.AddUtterance("Hali");
        greetingInformal.AddUtterance("Heló");
        greetingInformal.AddUtterance("Cső");
        greetingInformal.AddUtterance("Szió");
        greetingInformal.AddUtterance("Szevasz");
        greetingInformal.AddUtterance("Szervusz");

        greetingInformal.AddAnswer(
            "Szia! Miben segíthetek?");

        Intent greetingFormal =
            await GetOrCreateIntentAsync(
                context,
                "greeting_formal",
                "Magázó Köszönés",
                cancellationToken);

        greetingFormal.AddUtterance("Üdvözlöm");
        greetingFormal.AddUtterance("Üdvözletem");
        greetingFormal.AddUtterance("Jó reggelt");
        greetingFormal.AddUtterance("Jó napot");
        greetingFormal.AddUtterance("Jó estét");
        greetingFormal.AddUtterance("Jó reggelt kívánok");
        greetingFormal.AddUtterance("Jó napot kívánok");
        greetingFormal.AddUtterance("Jó estét kívánok");
        greetingFormal.AddUtterance("Szép jó reggelt kívánok");
        greetingFormal.AddUtterance("Szép jó napot kívánok");
        greetingFormal.AddUtterance("Szép jó estét kívánok");
        greetingFormal.AddUtterance("Kellemes jó reggelt kívánok");
        greetingFormal.AddUtterance("Kellemes jó napot kívánok");
        greetingFormal.AddUtterance("Kellemes jó estét kívánok");
        greetingFormal.AddUtterance("Tiszteletem");
        
        greetingFormal.AddAnswer("Jó napot kívánok! Miben segíthetek?");

        Intent goodbye =
    await GetOrCreateIntentAsync(
        context,
        "goodbye",
        "Búcsúzás",
        cancellationToken);

        goodbye.AddUtterance("Viszlát");
        goodbye.AddUtterance("Viszontlátásra");
        goodbye.AddUtterance("További szép napot");
        goodbye.AddUtterance("Elköszönök");
        goodbye.AddUtterance("Búcsúzom");
        goodbye.AddUtterance("Mára befejeztem");

        goodbye.AddAnswer(
            "Viszontlátásra! Remélem, hamarosan újra találkozunk!");

        Intent aboutSite = await GetOrCreateIntentAsync(
                                context,
                                "about_site",
                                "Az oldal bemutatása",
                                cancellationToken);

        aboutSite.AddUtterance("Mi ez az oldal?");
        aboutSite.AddUtterance("Miről szól ez az oldal?");
        aboutSite.AddUtterance("Mire való ez a weboldal?");
        aboutSite.AddUtterance("Mit lehet itt csinálni?");
        aboutSite.AddUtterance("Mi a Gyökereim?");
        aboutSite.AddUtterance("Miben tud segíteni ez az oldal?");

        aboutSite.AddAnswer(
            "A Gyökereim oldal családfakutatással kapcsolatos információkat és szolgáltatásokat kínál.");

        Intent helpRequest = await GetOrCreateIntentAsync(
                                context,
                                "help_request",
                                "Segítségkérés",
                                cancellationToken);

        helpRequest.AddUtterance("Segíts");
        helpRequest.AddUtterance("Tudsz segíteni?");
        helpRequest.AddUtterance("Segítségre van szükségem");
        helpRequest.AddUtterance("Nem találom amit keresek");
        helpRequest.AddUtterance("Merre menjek?");
        helpRequest.AddUtterance("Mit kell csinálnom?");

        helpRequest.AddAnswer(
            "Természetesen. Írd meg, mit keresel az oldalon, és megpróbálok segíteni.");

        Intent servicePrice =
    await GetOrCreateIntentAsync(
        context,
        "service_price",
        "Szolgáltatás árának lekérdezése",
        cancellationToken);

        servicePrice.AddUtterance("Mennyibe kerül?");
        servicePrice.AddUtterance("Mi az ára?");
        servicePrice.AddUtterance("Mennyibe kerülnek?");
        servicePrice.AddUtterance("Milyen árban van?");
        servicePrice.AddUtterance("Mennyit kell fizetni?");
        servicePrice.AddUtterance("Mennyibe kerül a szolgáltatás?");
        servicePrice.AddUtterance("Mennyibe kerül a Kutató 5?");
        servicePrice.AddUtterance("Mi az ára a Kutató 5-nek?");
        servicePrice.AddUtterance("Mi az ára a Néveredetnek?");
        servicePrice.AddUtterance("Mennyibe kerül a Néveredet?");
        servicePrice.AddUtterance("Mennyibe kerülnek ezek a szolgáltatások?");
        servicePrice.AddUtterance("A Kutató 5-öt szeretném");

        Intent serviceSelect = 
            await GetOrCreateIntentAsync(
                context,
                "service_select",
                "Szolgáltatás kiválasztása",
                cancellationToken);

        serviceSelect.AddUtterance("A Kutató 5-öt választanám");
        serviceSelect.AddUtterance("A Kutató 6-ot választom");
        serviceSelect.AddUtterance("A Néveredetet választanám");
        serviceSelect.AddUtterance("Kutató 5 érdekel");
        serviceSelect.AddUtterance("Kutató 5 mellett döntöttem");
        serviceSelect.AddUtterance("Legyen a Kutató 6");
        serviceSelect.AddUtterance("Ezt választom");
        serviceSelect.AddUtterance("Ezt szeretném");
        serviceSelect.AddUtterance("Az elsőt választom");
        serviceSelect.AddUtterance("A másodikat választanám");

        Intent serviceDetails =
            await GetOrCreateIntentAsync(
                context,
                "service_details",
                "Szolgáltatás részleteinek lekérdezése",
                cancellationToken);

        serviceDetails.AddUtterance("Mit tartalmaz?");
        serviceDetails.AddUtterance("Mi van benne?");
        serviceDetails.AddUtterance("Miről szól?");
        serviceDetails.AddUtterance("Mit tartalmaz a Kutató 5?");
        serviceDetails.AddUtterance("Miről szól a Kutató 6?");
        serviceDetails.AddUtterance("Mi van a Néveredet szolgáltatásban?");
        serviceDetails.AddUtterance("Mit kapok a Kutató 5-tel?");
        serviceDetails.AddUtterance("Mit foglal magában?");
        serviceDetails.AddUtterance("Mit kapok ezért?");
        serviceDetails.AddUtterance("Mi tartozik bele?");
        serviceDetails.AddUtterance("Mit tartalmaz ez a szolgáltatás?");
        serviceDetails.AddUtterance("Megtudhatok többet erről?");
        serviceDetails.AddUtterance("Erről tudna többet mondani?");

        Intent familyTreeResearch =
    await GetOrCreateIntentAsync(
        context,
        "family_tree_research",
        "Családfakutatás iránti érdeklődés",
        cancellationToken);

        familyTreeResearch.AddUtterance(
            "Szeretnék családfát készíteni");

        familyTreeResearch.AddUtterance(
            "Szeretném elkészíteni a családfámat");

        familyTreeResearch.AddUtterance(
            "Családfát szeretnék készíteni");

        familyTreeResearch.AddUtterance(
            "Szeretnék családfakutatásba kezdeni");

        familyTreeResearch.AddUtterance(
            "El szeretném kezdeni a családfakutatást");

        familyTreeResearch.AddUtterance(
            "Szeretném felkutatni a családom múltját");

        familyTreeResearch.AddUtterance(
            "Szeretném megismerni az őseimet");

        familyTreeResearch.AddUtterance(
            "Családfakutatás érdekel");

        familyTreeResearch.AddUtterance(
            "Családfát szeretnék");

        familyTreeResearch.AddUtterance(
            "Családfakutatást szeretnék");

        familyTreeResearch.AddUtterance(
            "Kutatni szeretném a családfámat");

        familyTreeResearch.AddUtterance(
            "Szeretném kutatni az őseimet");

        familyTreeResearch.AddUtterance(
            "Meg szeretném keresni az őseimet");

        familyTreeResearch.AddUtterance(
            "Érdekel a családom története");

        familyTreeResearch.AddUtterance(
            "Szeretném megtudni kik voltak az őseim");

        familyTreeResearch.AddAnswer(
            "Ehhez a Kutató 5, Kutató 6 és Néveredet szolgáltatásokat ajánlom.");

        Intent serviceOrder =
    await GetOrCreateIntentAsync(
        context,
        "service_order",
        "Szolgáltatás megrendelése",
        cancellationToken);

        serviceOrder.AddUtterance("Hol tudom megrendelni");
        serviceOrder.AddUtterance("Hol lehet megrendelni");
        serviceOrder.AddUtterance("Hogyan tudom megrendelni");
        serviceOrder.AddUtterance("Hogyan rendelhetem meg");
        serviceOrder.AddUtterance("Szeretném megrendelni");
        serviceOrder.AddUtterance("Meg szeretném rendelni");
        serviceOrder.AddUtterance("Hol rendelhetem meg");
        serviceOrder.AddUtterance("Hogyan lehet rendelni");
        serviceOrder.AddUtterance("Hol tudok rendelést leadni");
        serviceOrder.AddUtterance("Hol tudom leadni a rendelést");
        serviceOrder.AddUtterance("Hol adhatom le a rendelést");

        Intent pedigreeCollapse =
            await GetOrCreateIntentAsync(
                context,
                "pedigree_collapse",
                "Ősvesztés (Tudományos)",
                cancellationToken);

        pedigreeCollapse.AddUtterance("Mi az ősvesztés");
        pedigreeCollapse.AddUtterance("Mit jelent az ősvesztés");
        pedigreeCollapse.AddUtterance("Mi az a pedigree collapse");
        pedigreeCollapse.AddUtterance("Miért kevesebb a valódi ősök száma");
        pedigreeCollapse.AddUtterance("Hogyan lehet, hogy ugyanaz az ős többször szerepel");
        pedigreeCollapse.AddUtterance("Miért ismétlődnek az őseim a családfában");
        pedigreeCollapse.AddUtterance("Mit jelent, ha ugyanaz a személy több ágon is ős");


        pedigreeCollapse.AddAnswer(
            "Az ősvesztés azt jelenti, hogy ugyanaz a személy több különböző ágon is ősként jelenik meg a családfában. Emiatt a tényleges, különálló ősök száma kisebb annál, mint amit az egyszerű generációnkénti duplázódás alapján várnánk.\r\n\r\nEz főleg olyan esetekben fordul elő, amikor egymással rokonságban álló emberek leszármazási vonalai később újra találkoznak. Kisebb településeken, zártabb közösségekben és történelmi nemesi családoknál ez gyakori jelenség lehet.\r\n\r\nAz ősvesztés angol neve: pedigree collapse.");

        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task<Intent> GetOrCreateIntentAsync(
    ChatDbContext context,
    string key,
    string name,
    CancellationToken cancellationToken)
    {
        Intent? intent = await context.Intents
            .Include(x => x.Utterances)
            .Include(x => x.Answers)
            .FirstOrDefaultAsync(
                x => x.Key == key,
                cancellationToken);

        if (intent is not null)
        {
            return intent;
        }

        intent = new Intent
        {
            Key = key,
            Name = name
        };

        context.Intents.Add(intent);

        return intent;
    }
}
