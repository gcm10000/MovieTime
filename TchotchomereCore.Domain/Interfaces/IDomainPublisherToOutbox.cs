namespace TchotchomereCore.Domain.Interfaces;
public interface IDomainPublisherToOutbox
{
    void PublishDomainEvent<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent;
}
