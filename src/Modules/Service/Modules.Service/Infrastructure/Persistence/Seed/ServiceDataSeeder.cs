namespace Modules.Service.Infrastructure.Persistence.Seed;

internal sealed class ServiceDataSeeder
{
    internal async Task SeedAsync(
        ServiceDbContext context,
        CancellationToken cancellationToken = default)
    {
        if (await context.Services.AnyAsync(cancellationToken))
        {
            return;
        }
        var kutato5 = Domain.Service.Create(
            "Kutató 5",
            "Maximum 32 fő (magunkat nem beleszámítva). Előleg szükséges, melynek értéke: a teljes ár 20%-a",
            "A Kutató 5 egy olyan szolgáltatás, amely maximum 32 fő kutatása (magunkat nem beleszámítva). Előleg szükséges, melynek értéke: a teljes ár 20%-a.",
            "kutato-5",
            144000,
            1,
            ServiceStatusCode.Active
        );
        var kutato6 = Domain.Service.Create(
            "Kutató 6",
            "Maximum 64 fő (magunkat nem beleszámítva). Előleg szükséges, melynek értéke: a teljes ár 20%-a",
            "A Kutató 6 egy olyan szolgáltatás, amely maximum 64 fő kutatása (magunkat nem beleszámítva). Előleg szükséges, melynek értéke: a teljes ár 20%-a.",
            "kutato-6",
            288000,
            2,
            ServiceStatusCode.Active
        );
        var kutato7 = Domain.Service.Create(
            "Kutató 7",
            "Maximum 128 fő (magunkat nem beleszámítva). Előleg szükséges, melynek értéke: a teljes ár 20%-a",
            "A Kutató 7 egy olyan szolgáltatás, amely maximum 128 fő kutatása (magunkat nem beleszámítva). Előleg szükséges, melynek értéke: a teljes ár 20%-a.",
            "kutato-7",
            576000,
            3,
            ServiceStatusCode.Active
        );
        var neveredet = Domain.Service.Create(
            "Néveredet",
            "Névviselő kiválasztott ág kutatása, a legkorábbi anyakönyvi forrásig visszakutatva. Előleg szükséges, melynek értéke: a teljes ár 20%-a",
            "Névviselő kiválasztott ág kutatása, a legkorábbi anyakönyvi forrásig visszakutatva. Történelmi viszonyokat figyelembe véve maximum 1700-as évek elejéig lehet visszavezetni.",
            "neveredet",
            576000,
            4,
            ServiceStatusCode.Active
        );
        await context.Services.AddRangeAsync(kutato5, kutato6, kutato7, neveredet);
        await context.SaveChangesAsync(cancellationToken);
    }
}
