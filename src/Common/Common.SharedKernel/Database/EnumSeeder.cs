namespace Common.SharedKernel.Database;

public abstract class EnumSeeder(DbContext context)
{
    protected TEntity SeedEnum<TEnum, TEntity>(TEnum enumItem, string displayName)
        where TEnum : Enum
        where TEntity : EnumEntity<TEnum>, new()
    {
        Ensure.NotNull(context);

        TEntity? entity = context.Set<TEntity>().FirstOrDefault(x => Convert.ToInt32(x.Id) == Convert.ToInt32(enumItem));
        if (entity is null)
        {
            entity = new TEntity
            {
                Id = enumItem
            };
            context.Set<TEntity>().Add(entity);
        }
        entity.Name = displayName;
        entity.NormalizedName = displayName.ToUpperInvariant();
        return entity;
    }
}
