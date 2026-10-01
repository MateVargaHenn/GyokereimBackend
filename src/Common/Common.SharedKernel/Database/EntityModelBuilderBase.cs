namespace Common.SharedKernel.Database;

public abstract class EntityModelBuilderBase
{
    private readonly string _dateTimeType = "timestamp with time zone";

    protected void ConfigureCommonProperties<T>(EntityTypeBuilder<T> builder) where T : Entity
    {
        builder.Property(e => e.LastModified)
            .HasColumnType(_dateTimeType)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd()
            .IsRequired();
        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100)
            .HasDefaultValue("System")
            .ValueGeneratedOnAdd()
            .IsRequired();
    }

    protected static void ConfigureCommonEnumProperties<TEntity, TEnum>(
    EntityTypeBuilder<TEntity> builder)
    where TEntity : EnumEntity<TEnum>
    where TEnum : struct, Enum
    {
        builder.HasKey(x => x.Id);
        builder.Property(e => e.ConcurrencyStamp)
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Name).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.NormalizedName).HasComputedColumnSql("UPPER(\"Name\")", stored: true);
    }
}
