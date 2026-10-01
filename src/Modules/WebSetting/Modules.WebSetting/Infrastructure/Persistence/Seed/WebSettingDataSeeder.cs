
namespace Modules.WebSetting.Infrastructure.Persistence.Seed;

internal class WebSettingDataSeeder
{
    internal async Task SeedAsync(
        WebSettingDbContext context,
        CancellationToken cancellationToken = default)
    {
        Domain.WebSetting[] webSettings = new[]
        {
            new Domain.WebSetting
            {
                Key = "introduce",
                Value = "<p>Bánfi Zoltán Csaba vagyok, családfakutató, történész és üzleti intelligencia elemző.</p> <p>Történelem szakon végeztem a Pázmány Péter Katolikus Egyetemen, majd nemzetközi tanulmányok és kommunikáció szakon folytattam tanulmányaimat.</p> <p>Jelenleg üzleti intelligencia elemzőként dolgozom, ahol adatmodellezéssel, adatelemzéssel és üzleti riportok készítésével foglalkozom.</p><p>A családtörténet kutatása évek óta szenvedélyem. A genealógiában különösen vonz a történelmi források feltárása, az anyakönyvek és levéltári dokumentumok elemzése, valamint azoknak a történeteknek a megismerése, amelyek őseink életét formálták.</p> <p>Történészi képzettségem és adatközpontú szemléletem egyaránt segít abban, hogy a családfakutatást rendszerszemlélettel és a forráskritika iránti igényességgel végezzem.</p><p>Kutatásaim során elsősorban a történelmi Magyarországhoz kapcsolódó levéltári és egyházi forrásokkal dolgozom. Ezen kívül kutattam már lengyel, cseh és német anyakönyvekben is.</p>"
            },
            new Domain.WebSetting
            {
                Key = "introduce_title",
                Value = "Bánfi Zoltán Csaba"
            },
            new Domain.WebSetting
            {
                Key = "introduce_subtitle",
                Value = "Családfakutató, történész és üzleti intelligencia elemző"
            },
            new Domain.WebSetting
            {
                Key = "introduce_quote",
                Value = "A múlt történeteinek feltárása számomra nemcsak kutatás, hanem annak megértése is, hogyan formálták őseink a jelent."
            },
            new Domain.WebSetting
            {
                Key = "introduce_short",
                Value = "<p>Bánfi Zoltán Csaba vagyok, családfakutató, történész és üzleti intelligencia elemző.</p>"
            },
            new Domain.WebSetting
            {
                Key = "introduce_image",
                Value = "about.jpg"
            }
        };

        List<string> existingKeys = await context.WebSettings
            .Select(x => x.Key)
            .ToListAsync(cancellationToken);

        var missingWebSettings = webSettings
            .Where(x => !existingKeys.Contains(x.Key))
            .ToList();

        if (missingWebSettings.Count == 0)
        {
            return;
        }

        context.WebSettings.AddRange(missingWebSettings);

        await context.SaveChangesAsync(cancellationToken);
    }
}
