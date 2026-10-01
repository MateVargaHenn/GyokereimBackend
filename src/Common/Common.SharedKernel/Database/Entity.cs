namespace Common.SharedKernel.Database;

public abstract class Entity : IBaseModificationDataEntity
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
    public string ModifiedBy { get; set; } = "System";

    private readonly List<IDomainEvent> _domainEvents = [];

    public ICollection<IDomainEvent> DomainEvents => [.. _domainEvents];

    public void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
