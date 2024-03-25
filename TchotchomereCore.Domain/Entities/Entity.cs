using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TchotchomereCore.Domain.Interfaces;

namespace TchotchomereCore.Domain.Entities;

public abstract class Entity
{
    [Key]
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }
    public DateTime? CreatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string? LastModifiedBy { get; private set; }

    public Entity(Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    protected void Raise(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();

    public void SetInformationCreate(string? createdBy)
    {
        CreatedBy = createdBy;
    }

    public void SetInformationUpdate(DateTime? updatedAt, string? lastModifiedBy)
    {
        UpdatedAt = updatedAt;
        LastModifiedBy = lastModifiedBy;
    }
}
