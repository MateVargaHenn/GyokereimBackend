namespace Common.SharedKernel.Database;

public abstract class EnumEntity<TEnum> where TEnum : Enum
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    public TEnum Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string NormalizedName { get; set; } = default!;
    public Guid ConcurrencyStamp { get; set; } = Guid.NewGuid();
    public override string ToString() => Name;
}
